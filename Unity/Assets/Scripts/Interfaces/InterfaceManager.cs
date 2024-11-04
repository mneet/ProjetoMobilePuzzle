using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject activeMenu;

    public void ChangeMenu(GameObject menu)
    {
        menu.SetActive(true);
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = menu;
    }

    public void PlayScene(string sceneName)
    {
        if (sceneName != null && sceneName != "")
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    public void RestartRoom()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();       
    }
}
