using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        // Player? KILL
        PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            Debug.Log("Player " + player.PlayerIndex + " died on trap!");

            player.Die();
        }
    }
}
