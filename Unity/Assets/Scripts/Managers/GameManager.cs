using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public bool pause = false;

    [Header("Object References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hudMenu;

    [Header("Audio")]
    [SerializeField] private string winLevelSound = "level_win";
    [SerializeField] private string defeatLevelSound = "level_lose";

    public void PauseGame()
    {        
        pause = !pause;
        if (pause)
        {
            Debug.Log("Pausing game");
            HudManager.Instance.ChangeMenu(pauseMenu);       
        }
        else
        {
            Debug.Log("Resuming game");
            HudManager.Instance.ChangeMenu(hudMenu);        
        }
    }


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

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion

}
