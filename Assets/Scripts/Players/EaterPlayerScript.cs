using UnityEngine;
using System.Collections;

/// <summary>
/// Rabbit trying to eat coins in the dark
/// </summary>
public class EaterPlayerScript : PlayerScript
{
    public float iaDirectionCooldownMin = 1f, iaDirectionCooldownMax = 4f;

    private bool mIsIALaunch;
    private Vector3 mIADirection = Vector3.zero;

    private EatHuntGameScript mGame;

    void Start()
    {
        IsDead = false;

        mGame = GameObject.FindObjectOfType(typeof(EatHuntGameScript)) as EatHuntGameScript;
        if (mGame == null)
        {
            Debug.LogError("Invalid context, there must be an EatHuntGameScript script in the scene");
        }
    }

    void Update()
    {
        if (mIsIALaunch == false)
        {
            if (mGame.IsStarted && mGame.IsEnded == false)
            {
                mIsIALaunch = true;

                // Wait a few sec for each bot
                StartCoroutine(
                    Timers.Start(Random.Range(2f, 10f),
                    () =>
                    {
                        // Then move
                        StartCoroutine(IARandomMove());
                    })
                );
            }
        }

        if (IA == false)
        {
            UpdatePlayer();
        }
        else
        {
            UpdateIA();
        }
    }

    /// <summary>
    /// Inputs from human
    /// </summary>
    private void UpdatePlayer()
    {
        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);

        Vector3 movement = new Vector3(
            x * Speed * GameTimeScript.DeltaTime,
            y * Speed * GameTimeScript.DeltaTime,
            0);

        Move(movement);
    }

    /// <summary>
    /// Random movement from an "IA"
    /// </summary>
    private void UpdateIA()
    {
        Vector3 movement = new Vector3(
            mIADirection.x * Speed * GameTimeScript.DeltaTime,
            mIADirection.y * Speed * GameTimeScript.DeltaTime,
            0);

        Move(movement);
    }

    /// <summary>
    /// Move the player
    /// </summary>
    /// <param name="movement"></param>
    private void Move(Vector3 movement)
    {
        // Kill inertia
        rigidbody.velocity = Vector3.zero;

        rigidbody.MovePosition(rigidbody.position + movement);

    }

    /// <summary>
    /// Random perdiodic direction for IA
    /// </summary>
    /// <returns></returns>
    private IEnumerator IARandomMove()
    {
        while (true)
        {
            // Take a random direction
            mIADirection = new Vector3(
                Random.Range(-1, 2), // INTEGERS, BITCH
                Random.Range(-1, 2),
                0);

            float waitingTime = Random.Range(iaDirectionCooldownMin, iaDirectionCooldownMax);

            // Wait a random amount of time
            yield return new WaitForSeconds(waitingTime);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (IA == false)
        {
            // Coin?
            CoinInFogScript coin = collider.gameObject.GetComponent<CoinInFogScript>();
            if (coin != null)
            {
                coin.EatBy(this);

                // Score up
            }
        }
    }

    /// <summary>
    /// Poor little rabbit is dead
    /// </summary>
    public void Die()
    {
        if (IsDead == false)
        {
            IsDead = true;

            // Change sprite to a dead one

            // Disable the script. Bye.
            this.enabled = false;
        }
    }

    public bool IsDead { get; private set; }
}
