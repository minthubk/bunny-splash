using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HunterPlayerScript : PlayerScript
{
    public float FireRate = 2f;

    private bool mCanFire;
    private ParticleControlScript mBlood;
    private List<EaterPlayerScript> mHumanPlayersLeft;

    private EatHuntGameScript mGame;

    void Start()
    {
        mGame = GameObject.FindObjectOfType(typeof(EatHuntGameScript)) as EatHuntGameScript;
        if (mGame == null)
        {
            Debug.LogError("Invalid context, there must be an EatHuntGameScript script in the scene");
        }

        mCanFire = true;

        mHumanPlayersLeft = new List<EaterPlayerScript>();

        // Get Eaters
        foreach (var e in GameObject.FindObjectsOfType(typeof(EaterPlayerScript)))
        {
            EaterPlayerScript eater = e as EaterPlayerScript;

            // Disable collision with them
            Physics.IgnoreCollision(collider, eater.collider);

            if (eater.IA == false)
            {
                mHumanPlayersLeft.Add(eater);
            }
        }

        mBlood = GetComponent<ParticleControlScript>();
        if (mBlood == null) Debug.LogError("Missing blood!");
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);
        float action = Input.GetAxis("Action_Player" + PlayerIndex);

        Vector3 movement = new Vector3(
            x * Speed * GameTimeScript.DeltaTime,
            y * Speed * GameTimeScript.DeltaTime,
            0);

        transform.Translate(movement);

        if (action > 0 && mCanFire)
        {
            // Play sound
            SoundBankScript.Instance.Play(SoundBankScript.Instance.Jump[Random.Range(0, SoundBankScript.Instance.Jump.Count)]);

            // Launch timer
            mCanFire = false;

            StartCoroutine(Timers.Start(FireRate, () =>
            {
                mCanFire = true;
            }));

            // Try to kill
            Shoot();
        }
    }

    public void Shoot() {

        // Raycast to reach a player or something
        // Start the ray slightly behind
        var hits = Physics.RaycastAll(new Ray(transform.position + new Vector3(0,0,-0.8f), new Vector3(0, 0, 1)));

        foreach (var hit in hits)
        {
            Debug.Log(hit.transform);

            if (hit.collider != null)
            {
                EaterPlayerScript player = hit.collider.gameObject.GetComponent<EaterPlayerScript>();
                if (player != null && player.IsDead == false)
                {
                    // BLOOD
                    mBlood.Play(transform.position);

                    // DIE§§§
                    player.Die();

                    mGame.RemovePlayer(player);
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // Coin?
        CoinInFogScript coin = collider.gameObject.GetComponent<CoinInFogScript>();
        if (coin != null)
        {
            coin.SetIlluminated(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Hunter?
        CoinInFogScript coin = collider.gameObject.GetComponent<CoinInFogScript>();
        if (coin != null)
        {
            coin.SetIlluminated(false);
        }
    }
}
