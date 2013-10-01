using UnityEngine;
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

    void Start()
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

        // Spawn stuff
        //------------------------------------------------------------------------
        for (int i = 0; i < CoinCount; i++)
        {
            Transform coin = GameObject.Instantiate(CoinPrefab) as Transform;
            coin.position = RandomLocation(0);
        }
        for (int i = 0; i < EaterCount; i++)
        {
            Transform eater = GameObject.Instantiate(EaterPrefab) as Transform;
            eater.position = RandomLocation(-0.1f);

            EaterPlayerScript player = eater.GetComponent<EaterPlayerScript>();
            player.Initialize(HunterCount + i + 1, false);
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

            // End timer
            //------------------------------------------------------------------------
            StartCoroutine(Timers.Start(TimeLeftBase, 1f, (t2) =>
            {
                mTimeLeft = TimeLeftBase - t2;
                TimeText.text = mTimeLeft.ToString("00");
            },
            (t2) =>
            {
                IsEnded = true;
                mTimeLeft = 0;
                WinnerText.enabled = true;
            }));
        }));

    }

    void Update()
    {
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

    public bool IsStarted { get; private set; }
    public bool IsEnded { get; private set; } 
}
