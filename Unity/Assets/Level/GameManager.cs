using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool pause = false;

    [Header("Object References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hudMenu;

    public void PauseGame()
    {
        pause = !pause;
        if (pause)
        {
            pauseMenu.SetActive(true);
            hudMenu.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            hudMenu.SetActive(true);
            Time.timeScale = 1;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
