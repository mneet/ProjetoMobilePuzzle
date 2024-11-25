using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenMenuScale : TweenInterface
{
    [SerializeField] private RectTransform[] menuElements;
    [SerializeField] private RectTransform menuPanel;

    [SerializeField] private float tweenTimeIn = 0.5f;
    [SerializeField] private float tweenTimeOut = 0.5f;
    [SerializeField] private float tweenDelay = 0.05f;

    private bool positionsSet = false;

    public void GetPositions()
    {
        if (positionsSet) return;
        positionsSet = true;

        menuPanel.LeanAlpha(0, 0);
        for (int i = 0; i < menuElements.Length; i++)
        {
            LeanTween.scale(menuElements[i], Vector3.zero, 0);
        }
    }

    public void TransitionIn()
    {
        Debug.Log("Transitioning Menu In");
        LeanTween.alpha(menuPanel, 1, tweenTimeIn);
        for (int i = 0; i < menuElements.Length; i++)
        {
            LeanTween.scale(menuElements[i], Vector3.one, tweenTimeIn).setEaseOutBounce().setDelay(tweenDelay * i);
        }     
    }

    public void TransitionOut(GameObject nextScreen)
    {
        for (int i = 0; i < menuElements.Length; i++)
        {
            LeanTween.scale(menuElements[i], Vector3.zero, tweenTimeOut).setEaseOutBounce().setDelay(tweenDelay * i);
        }
        LeanTween.alpha(menuPanel, 0, tweenTimeOut).setDelay(tweenDelay * menuElements.Length).setOnComplete(() =>
        {           
            if (InterfaceManager.Instance != null) InterfaceManager.Instance.ChangeMenu(nextScreen);
            else if (HudManager.Instance != null) HudManager.Instance.ChangeMenu(nextScreen);
        }
        );
    }

    public void TransitionOutPause()
    {
        for (int i = 0; i < menuElements.Length; i++)
        {
            LeanTween.scale(menuElements[i], Vector3.zero, tweenTimeOut).setEaseOutBounce().setDelay(tweenDelay * i);
        }
        LeanTween.alpha(menuPanel, 0, tweenTimeOut).setDelay(tweenDelay * menuElements.Length).setOnComplete(() =>
        {
            GameManager.Instance.PauseGame();
        }
        );
    }

    private void OnEnable()
    {
        GetPositions();
        TransitionIn();
    }

}
