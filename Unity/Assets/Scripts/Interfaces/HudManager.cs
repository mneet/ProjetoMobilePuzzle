using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudManager : InterfaceManager
{
    static public HudManager Instance;

    [Header("Object References")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject endgamePanel;
    [SerializeField] private GameObject defeatPanel;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject score;

    [Header("Score Bars")]
    [SerializeField] private GameObject[] scoreBars;
    [SerializeField] private Sprite scoreBarWin;
    [SerializeField] private Sprite scoreBarLose;

    public void updateTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60F);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void callEndgamePanel()
    {
        colorScoreBars();
        ChangeMenu(endgamePanel);
    }

    public void callDefeatPanel()
    {
        ChangeMenu(defeatPanel);
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    public void RotateSelectedTile(int dir)
    {
        GridMap.Instance.RotateSelectedTile(dir);
    }

    public void ClearSelectedTile()
    {
        GridMap.Instance.ClearSelectedTile();
    }

    private void colorScoreBars()
    {
        for (int i = 0; i < LevelManager.Instance.score; i++)
        {
            scoreBars[i].GetComponent<Image>().sprite = scoreBarWin;
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
            Destroy(this);
        }
    }
}
