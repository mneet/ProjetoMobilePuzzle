using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public static GridMap Instance;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase emptyTile;

    public GameObject prefab1;

    private Tile tileToPlace;

    public GameObject tileDragged = null;
    private GameObject tileHighlighted = null;

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(prefab1);
        } 
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = GetMouseWorldPosition();
            Vector3Int gridPos = gridLayout.WorldToCell(pos);
            Debug.Log($"{MainTilemap.GetTile(gridPos)} | {gridPos}");
        }
    }

    #endregion

    #region Utils

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayHit) )
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

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        tileToPlace = obj.GetComponent<Tile>();
        obj.AddComponent<TileDrag>();
    }

    #endregion
}
