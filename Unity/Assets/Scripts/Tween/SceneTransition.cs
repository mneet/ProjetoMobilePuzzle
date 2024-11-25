using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTimer = 0.5f;

    public void transitionFade(int sceneIndex)
    {
        StartCoroutine(TrasitionWithFade(sceneIndex));
    }

    public void trasitionNextLevel()
    {
        StartCoroutine(TrasitionWithFade(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator TrasitionWithFade(int nextScene)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1, fadeTimer);
        yield return new WaitForSeconds(fadeTimer);
        SceneManager.LoadScene(nextScene);
        LeanTween.alpha(fadeImage.rectTransform, 0, fadeTimer);
    }

    private void Start()
    {
        LeanTween.alpha(fadeImage.rectTransform, 0, fadeTimer);
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
        DontDestroyOnLoad(this);
    }
}
