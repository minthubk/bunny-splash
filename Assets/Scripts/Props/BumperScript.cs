using UnityEngine;
using System.Collections;

public class BumperScript : MonoBehaviour
{
    public float Bounciness = 3000f;
    public float ReloadingRate = 0.5f;

    private float mReloadingTime;

    void Start()
    {
        mReloadingTime = 0f;
    }

    void Update()
    {
        if (mReloadingTime > 0f)
        {
            mReloadingTime -= Time.deltaTime;

            if (mReloadingTime <= 0f)
            {
                mReloadingTime = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (mReloadingTime == 0f)
        {
            if (otherCollider.gameObject.rigidbody != null)
            {
                // Sound
                SoundBankScript.Instance.Play(SoundBankScript.Instance.Jump[Random.Range(0, SoundBankScript.Instance.Jump.Count)]);

                // Get the bumper direction
                Vector3 direction = Vector3.Normalize(transform.rotation * Vector3.up);
                Vector3 bounce = new Vector3(Bounciness * direction.x, Bounciness * direction.y, 0f);

                // Cancel velocity
                otherCollider.gameObject.rigidbody.velocity = new Vector3(otherCollider.gameObject.rigidbody.velocity.x, 0f, 0f);

                // Apply bump force
                otherCollider.gameObject.rigidbody.AddForce(bounce);

                // if it's player, tell him
                PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();
                if (player != null)
                {
                    player.Bump();
                }
            }
        }
    }
}
