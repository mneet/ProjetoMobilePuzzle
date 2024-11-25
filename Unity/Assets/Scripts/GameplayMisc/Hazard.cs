using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private bool collided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collided)
        {
            other.GetComponent<Player>().ShakeCar();
            LevelManager.Instance.LoseCartOres();
            collided = true;
            AudioManager.Instance.PlayCarCrash();
        }
    }
}
