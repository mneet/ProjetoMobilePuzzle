using UnityEngine;

public class SideToSideWobble : MonoBehaviour
{
    public float wobbleIntensity = 0.2f;  // �ngulo m�ximo de rota��o para o balan�o
    public float wobbleFrequency = 1f;   // Velocidade do balan�o

    private Quaternion originalRotation;
    private float wobbleDuration = 1f;
    private float wobbleTimer = 0f;


    void Update()
    {
        // Se o temporizador de balan�o ainda estiver ativo, aplica o balan�o
          

    }

    // Fun��o para iniciar o efeito de balan�o com uma dura��o espec�fica
    public void StartWobble()
    {
        wobbleTimer = wobbleDuration;
    }

}
