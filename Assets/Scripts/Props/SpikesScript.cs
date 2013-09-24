using UnityEngine;
using System.Collections;

public class SpikesScript : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        // Player? KILL
        PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            // Only if on top, so just check if the player if falling
            // Remember the player may jump while in spikes and he shouldn't die
            Debug.Log(player.rigidbody.velocity);

            if (player.rigidbody.velocity.y < -1f)
            {
                Debug.Log("Player " + player.PlayerIndex + " died on spikes!");

                player.Die();
            }
        }
    }
}
