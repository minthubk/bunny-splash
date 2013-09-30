using UnityEngine;
using System.Collections;

/// <summary>
/// Generic player controller
/// </summary>
public abstract class PlayerScript : MonoBehaviour
{
    public int PlayerIndex = 1;
    public float Speed = 10f;

    public virtual void Initialize(int playerIndex)
    {
        PlayerIndex = playerIndex;
    }
}
