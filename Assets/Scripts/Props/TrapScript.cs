using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour
{
    public float ReloadingRate = 3f;

    private float mReloadingTime;

    void Update()
    {
        if (mReloadingTime > 0f)
        {
            mReloadingTime -= Time.deltaTime;

            if (mReloadingTime <= 0f)
            {
                mReloadingTime = 0f;

                // Sprite 1
                renderer.material.mainTextureOffset = new Vector2(0 * renderer.material.mainTextureScale.x, 0);
            }
            else
            {
                // Sprite 2
                renderer.material.mainTextureOffset = new Vector2(1 * renderer.material.mainTextureScale.x, 0);
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (mReloadingTime == 0f)
        {
            mReloadingTime = ReloadingRate;

            // Player? KILL?
            PlayerScript player = collisionInfo.collider.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
