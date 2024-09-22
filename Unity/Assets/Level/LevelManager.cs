using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Counters")]
    [SerializeField] private float score = 0f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float resources = 5f;

    
    public enum LevelState
    {
        START,
        PUZZLE,
        PATH_FOLLOWING,
        END
    }
    [Header("Level Flags")]
    [SerializeField] private LevelState state = LevelState.START;
    private bool levelStarted = false;
    private bool levelEnded = false;
    private bool levelVictory = false;

    [Header("Object References")]
    [SerializeField] private GameObject player;

    #region Level Management Methods
    private void LevelStateController()
    {
        switch (state)
        {
            case LevelState.PUZZLE:
                timer += Time.deltaTime;
                if (HudManager.Instance != null)
                {
                    HudManager.Instance.updateTimer(timer);
                }
                break;
        }
    }

    public void PuzzleComplete()
    {
        if (state == LevelState.PUZZLE)
        {
            state = LevelState.PATH_FOLLOWING;
            if (player != null) player.GetComponent<Player>().followWaypoints = true;
            Debug.Log("Puzzle completed");
        }
    }
    public void GoalReached()
    {
        levelVictory = true;
        state = LevelState.END;
        Debug.Log("Player cart reached the goal");

        if (HudManager.Instance != null)
        {
            HudManager.Instance.callEndgamePanel();
        }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        LevelStateController();
    }
    #endregion
}
