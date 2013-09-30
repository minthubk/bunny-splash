using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Game script
/// </summary>
public class GameScript : MonoBehaviour
{
    public static Dictionary<int,int> PlayerScores;

    public float TimeLeftBase = 60f;
    public Transform PlayerPrefab;

    public GUIText StartLabel, WinnerLabel;
    public GUIText P1scoreLabel, P2scoreLabel, P3scoreLabel, P4scoreLabel, TimeLabel;

    private bool mIsStarted, mIsEnded;
    private float mStartCooldown;
    private float mEndCooldown;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Timeleft = TimeLeftBase;

        if (Debug.isDebugBuild)
        {
            mStartCooldown = 1f;
        }
        else
        {
            mStartCooldown = 4f;
        }
        mIsStarted = false;
        mIsEnded = false;

        WinnerLabel.enabled = false;
    }

    void Update()
    {
        // START
        //---------------------------------------------------------------
        if (mIsStarted == false)
        {
            int cooldownAsIntBEfore = (int)mStartCooldown;
            mStartCooldown -= GameTimeScript.DeltaTime;

            int currentStartCooldown = (int)mStartCooldown;

            if (currentStartCooldown != cooldownAsIntBEfore)
            {
                // A full second as passed
                SoundBankScript.Instance.Play(SoundBankScript.Instance.Eat[Random.Range(0, SoundBankScript.Instance.Eat.Count)]);

                if (currentStartCooldown > 0)
                {
                    StartLabel.text = "" + currentStartCooldown;
                }
                else
                {
                    StartLabel.text = "FIGHT!";
                }
            }

            // Time is over
            if (mStartCooldown <= 0f)
            {
                mIsStarted = true;
                StartLabel.enabled = false;

                StartFight(4);
            }
        }

        // GAME
        //---------------------------------------------------------------
        if (mIsStarted && mIsEnded == false)
        {
            Timeleft -= GameTimeScript.DeltaTime;

            if (Timeleft <= 0f)
            {
                mIsEnded = true;

                EndFight();

                StartLabel.text = "Game Over";
                StartLabel.enabled = true;

                WinnerLabel.enabled = true;
                WinnerLabel.text = "Player " + GetLeadingPlayer() + " wins!";
            }
        }
        

        // END
        //---------------------------------------------------------------
        if (mIsEnded)
        {
            if (mEndCooldown > 0f)
            {
                mEndCooldown -= GameTimeScript.DeltaTime;
            }
            else
            {
                // Wait for a reset
                if (Input.anyKey)
                {
                    // Restart
                    Initialize();
                }
            }
        }
    }

    void OnGUI()
    {
        if (mIsStarted)
        {
            // Adjust scores
            P1scoreLabel.text = PlayerScores[1].ToString();
            P2scoreLabel.text = PlayerScores[2].ToString();
            P3scoreLabel.text = PlayerScores[3].ToString();
            P4scoreLabel.text = PlayerScores[4].ToString();

            // And time
            TimeLabel.text = Timeleft.ToString("00:00");
        }
    }

    public void StartFight(int playerNumber)
    {
        Debug.Log("START");

        // Create scores
        PlayerScores = new Dictionary<int, int>();

        // Create players
        for (int i = 1; i <= playerNumber; i++)
        {
            Transform playerTransform = GameObject.Instantiate(PlayerPrefab) as Transform;

            PlayerScores.Add(i, 0);

            PlayerScript script = playerTransform.gameObject.GetComponent<PlayerScript>();
            script.Initialize(i);
        }
    }

    public void EndFight()
    {
        Debug.Log("GAME OVER");

        mEndCooldown = 2f;

        var players = GameObject.FindObjectsOfType(typeof(PlayerScript));
        foreach (var p in players)
        {
            PlayerScript player = p as PlayerScript;
            GameObject.Destroy(player.gameObject);
        }
    }

    public int GetLeadingPlayer()
    {
        return PlayerScores.OrderByDescending(p => p.Value).Select(p => p.Key).First();
    }

    public float Timeleft { get; private set; }
}
