using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Sine Wave Movement")]
    [SerializeField] private float amplitude = 0.2f;   // Height of the wave
    [SerializeField] private float frequency = 0.2f;   // Speed of the wave
    [SerializeField] private Vector3 direction = Vector3.up;
    [SerializeField] private float rotationSpeed = 0.5f;

    [Header("Spring Animation")]
    [SerializeField] private AnimationCurve springCurve;
    [SerializeField] private float springDuration = 2f;
    private float springTimer = 0f;

    [Header("System Flags")]  
    [SerializeField] private bool collided = false;
    [SerializeField] private bool destroyWhenCollected = true;
    private Vector3 startPosition;
    private bool animating = false;

    [Header("Object References")]
    [SerializeField] private GameObject gemModel;
    [SerializeField] private ParticleSystem gemParticles;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            if (LevelManager.Instance != null) LevelManager.Instance.CollectGem();
            collided = true;
        }
    }

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!animating) SineWaveMovement();
        RotateAroundY();

        if (collided)
        {
            AnimateCollect();
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
        }
    }

    private void RotateAroundY()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }

    private void SineWaveMovement()
    {
        springTimer += Time.deltaTime;
        float offset = amplitude * Mathf.Sin(springTimer * frequency);
        transform.position = startPosition + direction * offset;
    }

    private void PlayParticle()
    {
        AudioManager.Instance.PlaySoundEffect(0);
        gemParticles.Play();
    }

    private void AnimateCollect()
    {
        if (animating) return;
        animating = true;
        springTimer = 0f;

        
        Invoke("PlayParticle", 0.5f);
        // Move Up
        LeanTween.moveY(gameObject, transform.position.y + .3f, 0.5f).setEase(LeanTweenType.easeInSine);
        // Scale Down
        LeanTween.scale(gameObject, new Vector3(0.7f, 0.7f, 0.7f), .5f).setEase(LeanTweenType.easeInSine);

        // If Destroy is turned on, tween alpha
        if (destroyWhenCollected) LeanTween.alpha(gemModel, 0f, .5f).setDelay(0.5f).setEase(LeanTweenType.easeInSine);

        // Spring back to original scale
        LeanTween.scale(gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(.5f).setOnComplete(() =>
        {
            
            if (destroyWhenCollected)
            {
                Destroy(gameObject);
            }
            else if (!destroyWhenCollected)
            {
                LeanTween.moveY(gameObject, startPosition.y, 1f).setDelay(1.2f).setEase(LeanTweenType.easeInSine).setOnComplete(() =>
                {
                    animating = false;
                    collided = false;
                });
            }
        });

    }
}
