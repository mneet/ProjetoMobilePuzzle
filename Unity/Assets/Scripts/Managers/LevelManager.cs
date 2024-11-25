using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Serializable]
    public struct LevelGoals
    {
        public int cartOres;
        public int gems;
        public int tilesSwap;
        public int tilesRotate;
        public float timer;
    }
    [Header("Level Parameters")]
    [SerializeField] private LevelGoals goals;
    [SerializeField] private LevelGoals counters;
    public int score = 3;
    
    public enum LevelState
    {
        START,
        PUZZLE,
        PATH_FOLLOWING,
        END
    }

    [Header("Level State")]
    [SerializeField] private LevelState state = LevelState.START;
    private bool levelStarted = false;
    private bool levelEnded = false;
    private bool levelVictory = false;

    [Header("Object References")]
    [SerializeField] private GameObject player;

    #region Level Goals
    public void CollectGem()
    {
        counters.gems++;
    }

    public void RegisterTileSwap()
    {
        counters.tilesSwap++;
    }

    public void RegisterTileRotated()
    {
        counters.tilesRotate++;
    }

    public void LoseCartOres()
    {
        counters.cartOres--;
    }
    #endregion

    #region Level Management Methods
    private void LevelStateController()
    {
        if (GameManager.Instance.pause) return;
        switch (state)
        {
            case LevelState.PUZZLE:
                counters.timer += Time.deltaTime;
                if (HudManager.Instance != null)
                {
                    HudManager.Instance.updateTimer(counters.timer);
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

    public void ReleaseCart()
    {
        if (state == LevelState.PUZZLE)
        {
            state = LevelState.PATH_FOLLOWING;
            if (player != null) player.GetComponent<Player>().followWaypoints = true;
            Debug.Log("Cart released");
        }
    }

    public void GoalReached()
    {
        Invoke("HandleGoalReached", 1f);
    }

    private void HandleGoalReached()
    {
        levelVictory = true;
        state = LevelState.END;

        CalculateScore();
        if (HudManager.Instance != null)
        {
            HudManager.Instance.callEndgamePanel();
        }
    }
    
    public void LoseLevel()
    {
        Invoke("HandleLoseLevel", 1f);
    }

    private void HandleLoseLevel()
    {
        levelVictory = false;
        state = LevelState.END;
        score = 0;
        if (HudManager.Instance != null)
        {
            HudManager.Instance.callDefeatPanel();
        }
    }

    public bool LevelInProgress()
    {
        return state == LevelState.PUZZLE;
    }

    private void CalculateScore()
    {
        if (counters.gems < goals.gems || counters.cartOres < goals.cartOres)
        {
            score--;
        }

        if (counters.tilesSwap > goals.tilesSwap || counters.tilesRotate > goals.tilesRotate)
        {
            score--;
        }

        if (counters.timer > goals.timer)
        {
            score--;
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
        counters.cartOres = goals.cartOres;
    }

    private void Start()
    {
        if (GridMap.Instance != null)
        {
            GridMap.Instance.RandomizeTiles(goals);
        }
    }

    private void Update()
    {
        LevelStateController();
    }
    #endregion
}
