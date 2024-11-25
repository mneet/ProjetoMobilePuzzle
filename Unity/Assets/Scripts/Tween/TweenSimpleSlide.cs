using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenSimpleSlide : TweenInterface
{
    [SerializeField] private RectTransform SlideMenu;

    [SerializeField] private float tweenTimeIn = 1f;
    [SerializeField] private float tweenTimeOut = 1f;

    private Vector2 slideMenuTarget;
    

    private bool positionsSet = false;

    public void GetPositions()
    {
        if (positionsSet) return;

        positionsSet = true;
        slideMenuTarget = SlideMenu.anchoredPosition;

        SlideMenu.anchoredPosition = new Vector2(2000, slideMenuTarget.y);
    }

    public void SlideIn()
    {
        LeanTween.move(SlideMenu, slideMenuTarget, tweenTimeIn).setEaseInSine();
    }

    public void SlideOut(GameObject nextScreen)
    {
        if (InterfaceManager.Instance.isTransitioning) return;
        InterfaceManager.Instance.isTransitioning = true;
        Vector2 slideMenuLeave = new Vector2(SlideMenu.anchoredPosition.x - 2000, SlideMenu.anchoredPosition.y);

        LeanTween.move(SlideMenu, slideMenuLeave, tweenTimeOut).setEaseInSine().setOnComplete(() =>
        {
            SlideMenu.anchoredPosition = new Vector2(2000, slideMenuTarget.y);
            Debug.Log("Leaving Slide In Simple");
            InterfaceManager.Instance.ChangeMenu(nextScreen);
        }
        );
    }

    private void OnEnable()
    {
        GetPositions();
        StartCoroutine(WaitUntilEndOfFrame());
    }

    private IEnumerator WaitUntilEndOfFrame()
    {
        yield return new WaitForEndOfFrame();       
        SlideIn();
    }
}
