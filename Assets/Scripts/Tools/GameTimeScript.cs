using UnityEngine;
using System.Collections;

public class GameTimeScript : MonoBehaviour
{
    public static float DeltaTime;
    public bool IsPaused = false;

    void Awake()
    {
        DeltaTime = 0f;
    }

    void Update()
    {
        if (IsPaused == false)
        {
            DeltaTime = Time.deltaTime;
        }
    }
}
