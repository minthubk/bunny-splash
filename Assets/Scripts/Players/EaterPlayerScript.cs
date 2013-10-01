using UnityEngine;
using System.Collections;

public class EaterPlayerScript : PlayerScript
{
    public float iaDirectionCooldownMin = 1f, iaDirectionCooldownMax = 4f;

    private bool mLaunchIA;
    private Vector3 direction = Vector3.zero;

    private EatHuntGameScript mGame;

    void Start()
    {
        mGame = GameObject.FindObjectOfType(typeof(EatHuntGameScript)) as EatHuntGameScript;
        if (mGame == null)
        {
            Debug.LogError("Invalid context, there must be an EatHuntGameScript script in the scene");
        }
    }

    void Update()
    {
        if (mLaunchIA == false)
        {
            if (mGame.IsStarted && mGame.IsEnded == false)
            {
                mLaunchIA = true;

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

    private void UpdateIA()
    {
        Vector3 movement = new Vector3(
            direction.x * Speed * GameTimeScript.DeltaTime,
            direction.y * Speed * GameTimeScript.DeltaTime,
            0);

        Move(movement);
    }

    /// <summary>
    /// Move the player
    /// </summary>
    /// <param name="movement"></param>
    private void Move(Vector3 movement)
    {
        transform.Translate(movement);
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
            direction = new Vector3(
                Random.Range(-1, 2), // INTEGERS, BITCH
                Random.Range(-1, 2),
                0);

            float waitingTime = Random.Range(iaDirectionCooldownMin, iaDirectionCooldownMax);

            // Wait a random amount of time
            yield return new WaitForSeconds(waitingTime);
        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        Debug.Log("POUET");

        // Coin?
        CoinInFogScript coin = collision.collider.gameObject.GetComponent<CoinInFogScript>();
        if (coin != null)
        {
            coin.EatBy(this);

            // Score up
        }
    }
}
