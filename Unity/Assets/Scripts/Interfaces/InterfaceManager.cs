using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    [SerializeField] private GameObject activeMenu;

    public bool isTransitioning = false;

    public void ChangeMenu(GameObject menu)
    {
        menu.SetActive(true);
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = menu;
        isTransitioning = false;
    }

    public void PlayScene(int nextScene)
    {
        if (nextScene != -1)
        {
            Time.timeScale = 1f;
            SceneTransition.Instance.transitionFade(nextScene);
        }
    }

    public void PlayNextLevel()
    {
        SceneTransition.Instance.trasitionNextLevel();
    }

    public void RestartRoom()
    {
        Time.timeScale = 1f;
        SceneTransition.Instance.transitionFade(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();       
    }
    
    public void AudioSetMainVolume(Slider slider)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateVolumeMaster(slider.value);
        }
    }

    public void AudioSetMusicVolume(Slider slider)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateVolumeBg(slider.value);
        }
    }

    public void AudioSetSFXVolume(Slider slider)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateVolumeSfx(slider.value);
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
}
