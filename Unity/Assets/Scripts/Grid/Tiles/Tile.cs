using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState
    {
        IDLE,
        SELECTED,
        MOVING
    }

    public enum PathType
    {
        STRAIGHT,
        CURVE
    }
    public PathType pathType = PathType.STRAIGHT;
    public TileState state = TileState.IDLE;

    private bool stateSet = false;
    private float stateDelay = 0f;
    private float sineTime = 0f;
    public bool Placed { get; private set; }
    public bool moveable = true;
    public bool rotatable = true;

    // Positions variables
    public Vector3 idlePosition;
    private Vector3 targetPosition;
    private Vector3 hoveringPositionOffset = new Vector3(0f, 0.5f, 0f);

    // Click Control
    private float clickTimer = 0f;
    private float clickRotTime = 0.0f;
    private float clickSelTime = 0.2f;
    private bool selectFlag = false;

    private bool isRotating = false;


    #region State Methods
    private void SetState(TileState newState)
    {
        state = newState;
        stateSet = false;
    }

    private void StateMachine()
    {
        switch (state)
        {
            case TileState.IDLE:
                if (!stateSet)
                {
                    stateSet = true;
                    LeanTween.move(gameObject, idlePosition, 0.2f).setEaseOutSine();
                }
                break;
            case TileState.SELECTED:
                if (!stateSet)
                {
                    sineTime = 0f;
                    stateDelay = 0.2f;
                    stateSet = true;
                    LeanTween.move(gameObject, idlePosition + hoveringPositionOffset, 0.2f).setEaseOutSine();
                }        

               
                if (stateDelay <= 0)
                {
                    sineTime += Time.deltaTime;
                    float offset = 0.05f * Mathf.Sin(sineTime * 3f);
                    transform.position = (idlePosition + hoveringPositionOffset) + Vector3.up * offset;
                } 
                else
                {
                    stateDelay -= Time.deltaTime;
                }

                break;
            case TileState.MOVING:
                if (!stateSet)
                {
                    stateSet = true;
                    LeanTween.move(gameObject, targetPosition, 0.2f).setEaseOutSine().setOnComplete(() =>
                    {
                        transform.position = GridMap.Instance.SnapCoordinateToGrid(transform.position);
                        SetState(TileState.IDLE);
                    });
                }

                break;
        }
    }

    #endregion

    #region Tile Click

    public void TileClickPressed()
    {
        if (GridMap.Instance.tileSelectedFlag && moveable)
        {
            GridMap.Instance.ClickTileSelect(this);
        }
    }

    public void TileClickActive()
    {
        if (moveable && state == TileState.IDLE)
        {
            clickTimer += Time.deltaTime;
            if (clickTimer >= clickSelTime && !selectFlag)
            {
                GridMap.Instance.ClickTileSelect(this);
            }
        }
    }

    public void TileClickReleased()
    {
        if (!GridMap.Instance.tileSelectedFlag && rotatable)
        {
            RotateTile();
        }
        clickTimer = 0f;
    }

    #endregion

    #region Utils

    public void RotateTile(int dir = 1)
    {
        if (isRotating) return;

        isRotating = true;
        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y + 60 * dir, 0.4f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
        {
            isRotating = false;
        });
    }

    public void RotateTileQuick(int dir = 1)
    {
        transform.Rotate(0, 60 * dir, 0);
    }

    public void SwapPositions(Vector3 target)
    {
        targetPosition = target;
        SetState(TileState.MOVING);
        selectFlag = false;
        idlePosition = targetPosition;
    }

    public void SelectTile()
    {
        SetState(TileState.SELECTED);
        selectFlag = true;
    }

    public void DeselectTile()
    {
        SetState(TileState.IDLE);
        selectFlag = false;
    }

    private void RandomizeRotation()
    {
        Waypath waypath = GetComponent<Waypath>();
        if (waypath != null && waypath.pathType == Waypath.PathType.START_LINE)
        {
            transform.Rotate(0, Random.Range(0, 6) * 60, 0); ;
        }      
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        idlePosition = pos;
    }

    #endregion

    #region Unity Methods
    private void Start()
    {
        transform.position = GridMap.Instance.SnapCoordinateToGrid(transform.position);   
        if (rotatable) RandomizeRotation();
    }

    private void Awake()
    {
        idlePosition = transform.position;
    }

    private void Update()
    {
        StateMachine();
    }

    #endregion
}

