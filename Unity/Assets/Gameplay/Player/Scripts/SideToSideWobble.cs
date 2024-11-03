using UnityEngine;

public class SideToSideWobble : MonoBehaviour
{
    public float wobbleIntensity = 0.2f;  // Ângulo máximo de rotação para o balanço
    public float wobbleFrequency = 1f;   // Velocidade do balanço

    private Quaternion originalRotation;
    private float wobbleDuration = 1f;
    private float wobbleTimer = 0f;


    void Update()
    {
        // Se o temporizador de balanço ainda estiver ativo, aplica o balanço
          

    }

    // Função para iniciar o efeito de balanço com uma duração específica
    public void StartWobble()
    {
        wobbleTimer = wobbleDuration;
    }

}
