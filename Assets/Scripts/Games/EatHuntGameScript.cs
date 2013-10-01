using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Second game mode, inspired by Hidden In Plain Sight
/// </summary>
public class EatHuntGameScript : MonoBehaviour
{
    public static Dictionary<int, int> PlayerScores;

    public float TimeLeftBase = 60f;
    public Transform EaterPrefab;
    public Transform HunterPrefab;
    public Transform CoinPrefab;

    public int CoinCount = 25;
    public int HunterCount = 2;
    public int EaterCount = 2;
    public int EaterIACount = 15;

    public GUIText StartText, WinnerText, TimeText;

    private float mTimeLeft;
    private float mEndCooldown;

    private List<CoinInFogScript> mCoinsLeft;
    private List<EaterPlayerScript> mHumanEatersLeft;

    void Start()
    {
        Initialize();

    }

    private void Initialize()
    {
        mTimeLeft = TimeLeftBase;

        StartText.text = "";
        StartText.enabled = true;
        WinnerText.text = "";
        WinnerText.enabled = false;
        TimeText.text = "";
        TimeText.enabled = false;

        IsStarted = false;
        IsEnded = false;

        mCoinsLeft = new List<CoinInFogScript>();

        // Remove all old stuff
        //------------------------------------------------------------------------
        foreach (var o in GameObject.FindObjectsOfType(typeof(HunterPlayerScript)))
        {
            HunterPlayerScript hunter = o as HunterPlayerScript;
            GameObject.Destroy(hunter.gameObject);
        }
        foreach (var o in GameObject.FindObjectsOfType(typeof(EaterPlayerScript)))
        {
            EaterPlayerScript eater = o as EaterPlayerScript;
            GameObject.Destroy(eater.gameObject);
        }
        foreach (var o in GameObject.FindObjectsOfType(typeof(CoinInFogScript)))
        {
            CoinInFogScript coin = o as CoinInFogScript;
            GameObject.Destroy(coin.gameObject);
        }

        // Spawn stuff
        //------------------------------------------------------------------------
        for (int i = 0; i < CoinCount; i++)
        {
            Transform coin = GameObject.Instantiate(CoinPrefab) as Transform;
            coin.position = RandomLocation(0);

            mCoinsLeft.Add(coin.GetComponent<CoinInFogScript>());
        }

        mHumanEatersLeft = new List<EaterPlayerScript>();

        for (int i = 0; i < EaterCount; i++)
        {
            Transform eater = GameObject.Instantiate(EaterPrefab) as Transform;
            eater.position = RandomLocation(-0.1f);

            EaterPlayerScript player = eater.GetComponent<EaterPlayerScript>();
            player.Initialize(HunterCount + i + 1, false);

            mHumanEatersLeft.Add(player);
        }

        for (int i = 0; i < EaterIACount; i++)
        {
            Transform ia = GameObject.Instantiate(EaterPrefab) as Transform;
            ia.position = RandomLocation(-0.1f);

            EaterPlayerScript player = ia.GetComponent<EaterPlayerScript>();
            player.Initialize(0, true);
        }

        // Hunter AFTER eaters because they need to get ALL eaters to ignore collisions
        for (int i = 0; i < HunterCount; i++)
        {
            Transform hunter = GameObject.Instantiate(HunterPrefab) as Transform;
            hunter.position = RandomLocation(-0.2f);

            HunterPlayerScript player = hunter.GetComponent<HunterPlayerScript>();
            player.Initialize(i + 1, false);
        }

        // Start cooldown
        //------------------------------------------------------------------------
        float startCooldown = 4f; // sec

        if (Debug.isDebugBuild)
        {
            startCooldown = 1;
        }

        StartCoroutine(Timers.Start(startCooldown, 1f, (t) =>
        {
            if (t < 3f)
            {
                StartText.text = (startCooldown - 1 - t).ToString("0");
            }
            else
            {
                StartText.text = ("FIGHT!");
            }
        },
        (t) =>
        {
            StartText.enabled = false;
            IsStarted = true;

            TimeText.enabled = true;
        }));
    }

    void Update()
    {
        // Ingame
        //------------------------------------------------------------------------
        if (IsStarted && IsEnded == false)
        {
            // Update timer
            mTimeLeft -= GameTimeScript.DeltaTime;
            TimeText.text = mTimeLeft.ToString("00");

            // Watch for time out
            if (mTimeLeft <= 0f)
            {
                End(false, false);
            }

            // Watch for a win
            if (mCoinsLeft.Count == 0)
            {
                End(true, false);
            }
            else if (mHumanEatersLeft.Count == 0)
            {
                End(false, true);
            }
        }

        // END
        //---------------------------------------------------------------
        if (IsEnded)
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

    /// <summary>
    /// End the game
    /// </summary>
    /// <param name="rabbits"></param>
    /// <param name="hunters"></param>
    public void End(bool rabbits, bool hunters)
    {
        IsEnded = true;

        mEndCooldown = 3f;

        string victoryText = "NOOBS";

        if (rabbits)
        {
            victoryText = "RABBITS WIN";
        }
        if (hunters)
        {
            victoryText = "HUNTERS WIN";
        }

        WinnerText.text = victoryText;
        mTimeLeft = 0;
        WinnerText.enabled = true;

        // Disable everyone
        foreach (var o in GameObject.FindObjectsOfType(typeof(HunterPlayerScript)))
        {
            HunterPlayerScript hunter = o as HunterPlayerScript;
            hunter.enabled = false;
        }
        foreach (var o in GameObject.FindObjectsOfType(typeof(EaterPlayerScript)))
        {
            EaterPlayerScript eater = o as EaterPlayerScript;
            eater.enabled = false;
        }
    }

    private Vector3 RandomLocation(float z)
    {
        // Get a random location on the map
        return new Vector3(
            Random.Range(-15, 15),
            Random.Range(-9, 9),
            z
            );
    }

    public void RemoveCoin(CoinInFogScript coin)
    {
        mCoinsLeft.Remove(coin);
    }

    public void RemovePlayer(EaterPlayerScript player)
    {
        mHumanEatersLeft.Remove(player);
    }

    public bool IsStarted { get; private set; }
    public bool IsEnded { get; private set; }
}
