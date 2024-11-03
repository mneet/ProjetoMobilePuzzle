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
    private Vector3 startPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            LevelManager.Instance.CollectGem();
            collided = true;
        }
    }

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        SineWaveMovement();
        RotateAroundY();

        if (collided)
        {
            AnimateCollect();
        }
    }

    private void RotateAroundY()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }

    private void SineWaveMovement()
    {
        float offset = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = startPosition + direction * offset;
    }

    private void AnimateCollect()
    {

        // Normaliza o tempo para que ele fique entre 0 e 1
        float normalizedTime = springTimer / springDuration;

        // Usa a curva para determinar o valor da escala neste ponto do tempo
        float scaleMultiplier = springCurve.Evaluate(normalizedTime);

        // Aplica a escala multiplicada
        transform.localScale = Vector3.one * scaleMultiplier;
            
        if (scaleMultiplier >= 1.1)
        {
            springTimer = springDuration;
        }
        // Incrementa o timer com o tempo decorrido
        springTimer += Time.deltaTime;
          
    }
}
