using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;
    [SerializeField] private bool start = false;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float magnitude = 1f;
    private void FixedUpdate() 
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());           
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShakeScreen()
    {
        StartCoroutine(Shake());  
    }
    IEnumerator Shake()
    {
        float time = 0;
        Vector3 originalPos = transform.position;
        while (time < duration)
        {
            time += Time.deltaTime;
            float curveValue = curve.Evaluate(time / duration);
            float x = Random.Range(-magnitude, magnitude) * curveValue;
            float y = Random.Range(-magnitude, magnitude) * curveValue;
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return null;
        }
        transform.position = originalPos;
    }
}
