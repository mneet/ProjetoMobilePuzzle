using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static LevelManager;

public class GridMap : MonoBehaviour
{
    public static GridMap Instance;

    public GridLayout gridLayout;
    private Grid grid;
    private static LayerMask tileLayer;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase emptyTile;

    public GameObject prefab1;

    private Tile tileToPlace;
    private Tile tileClicking;
    public GameObject tileDragged = null;
    private GameObject tileHighlighted = null;

    [SerializeField] private Tile tileSelected = null;
    public bool tileSelectedFlag = false;

    [SerializeField] private GameObject HexButton;

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        tileLayer = LayerMask.GetMask("Tiles");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    InitializeWithObject(prefab1);
        //}
        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Vector3 pos = GetMouseWorldPosition();
        //    Vector3Int gridPos = gridLayout.WorldToCell(pos);
        //    Debug.Log($"{MainTilemap.GetTile(gridPos)} | {gridPos}");
        //}

        RayCastMouseToTile();
        /*
        if (Input.touchCount > 0)
        {
            RayCastTouchToTile();
        }
        else
        {
            tileClicking = null;
        }
        */
    }


    #endregion

    #region Utils

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayHit))
        {
            if (rayHit.collider.gameObject.GetComponent<Tile>() != null && rayHit.collider.gameObject != tileDragged)
            {
                tileHighlighted = rayHit.collider.gameObject;
                Debug.Log("Tile Highlighted");
            }
            return rayHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;

    }

    #endregion

    #region Tile Placement

    public void RayCastTouchToTile()
    {
        if (LevelManager.Instance == null || !LevelManager.Instance.LevelInProgress() || GameManager.Instance.pause) return;

        Touch touch = Input.GetTouch(0);
        // Verifica se o toque est� na fase de toque inicial ou movido
        // Converte a posi��o do toque para um ray
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        // Raycast para a layer especificada
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, tileLayer))
        {
            Tile tileHit = raycastHit.collider.gameObject.GetComponent<Tile>();
            if (touch.phase == TouchPhase.Began)
            {
                if (tileHit.moveable || tileHit.rotatable)
                {
                    ClickTileSelect(tileHit);
                }                 
            }         
        }
    }

    public void RayCastMouseToTile()
    {
        if (LevelManager.Instance == null || !LevelManager.Instance.LevelInProgress() || GameManager.Instance.pause) 
            return;

        // Verifica se o botão esquerdo do mouse foi clicado
        if (Input.GetMouseButtonDown(0))
        {
            // Converte a posição do mouse para um ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast para a layer especificada
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, tileLayer))
            {
                Tile tileHit = raycastHit.collider.gameObject.GetComponent<Tile>();
                if (tileHit != null && (tileHit.moveable || tileHit.rotatable))
                {
                    ClickTileSelect(tileHit);
                }
            }
        }
    }


    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        tileToPlace = obj.GetComponent<Tile>();
        obj.AddComponent<TileDrag>();
    }
    
    public void ClearSelectedTile()
    {
        if (tileSelected != null)
        { 
            tileSelected.DeselectTile();
            tileSelected = null;
            tileSelectedFlag = false;
            HexButton.SetActive(false);
        }
    }
   
    public void ClickTileSelect(Tile tile)
    {
        if (tileSelected == null)
        {
            tileSelected = tile;
            tileSelectedFlag = true;
            tile.SelectTile();
            HexButton.SetActive(true);
        }
        else
        {
            if (tileSelected == tile)
            {
                ClearSelectedTile();
            }
            else if (tileSelected.moveable && tile.moveable)
            {
                Vector3 selectedTilePos = tileSelected.idlePosition;
                Vector3 swapTilePos = tile.idlePosition;
                tileSelected.SwapPositions(swapTilePos);
                tile.SwapPositions(selectedTilePos);

                ClearSelectedTile();
                LevelManager.Instance.RegisterTileSwap();
            }           
        }

    }

    public void RotateSelectedTile(int dir = 1)
    {
        if (tileSelected != null && tileSelected.rotatable)
        {
            tileSelected.RotateTile(dir);
            LevelManager.Instance.RegisterTileRotated();
        }
    }

    #endregion

    #region Randomize Level

    private List<Tile> ShuffleTileList(List<Tile> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Tile temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    private List<Tile> GetValidTileList(Tile[] tileArray)
    {
        List<Tile> validTiles = new List<Tile>();
        for (int i = 0; i < tileArray.Length; i++)
        {
            Tile tile = tileArray[i];
            if (tile.moveable)
            {
                validTiles.Add(tile);
            }
        }
        return validTiles;
    }

    private List<Tile> GetValidWaypathList(Waypath[] waypathArray)
    {
        List<Tile> validTiles = new List<Tile>();
        for (int i = 0; i < waypathArray.Length; i++)
        {
            Tile tile = waypathArray[i].GetComponent<Tile>();
            if (tile.moveable)
            {
                validTiles.Add(tile);
            }
        }
        return validTiles;
    }

    private int GetDifferentTile(Tile tile, List<Tile> tileList)
    {
        int tileFound = 0;
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i] != tile && tileList[i].pathType != tile.pathType)
            {
                tileFound =  i;
            }
        }
        return tileFound;
    }


     public void RandomizeTiles(LevelGoals goals)
    {
        Tile[] tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        Waypath[] pathTiles = FindObjectsByType<Waypath>(FindObjectsSortMode.None);

        List<Tile> pathTilesList = GetValidWaypathList(pathTiles);
        List<Tile> tileList = GetValidTileList(tiles);

        pathTilesList = ShuffleTileList(pathTilesList);
        tileList = ShuffleTileList(tileList);

        for (var i = 0; i < goals.tilesSwap; i++)
        {
            Tile firstTile = pathTilesList[0];

            int secondTileInd = GetDifferentTile(firstTile, tileList);
            Tile secondTile = tileList[secondTileInd];

            Vector3 firstTilePos = firstTile.idlePosition;

            firstTile.SetPosition(secondTile.idlePosition);
            secondTile.SetPosition(firstTilePos);

            pathTilesList.RemoveAt(0);
            pathTilesList.Add(firstTile);

            tileList.RemoveAt(secondTileInd);
            tileList.Add(secondTile);
        }

        for (var i = 0; i < goals.tilesRotate; i++)
        {
            Tile tile = pathTilesList[0];

            tile.RotateTileQuick(-1);

            pathTilesList.RemoveAt(0);
            pathTilesList.Add(tile);
        }
    }

    #endregion
}

