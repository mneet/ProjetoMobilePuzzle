using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : InterfaceManager
{
    static public HudManager Instance;

    [Header("Object References")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject endgamePanel;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject score;

    

    public void updateTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60F);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void callEndgamePanel()
    {
        endgamePanel.SetActive(true);
        hudPanel.SetActive(false);
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
