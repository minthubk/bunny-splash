using UnityEngine;
using System.Collections;

/// <summary>
/// Generic player controller
/// </summary>
public abstract class PlayerScript : MonoBehaviour
{
    public int PlayerIndex = 1;
    public float Speed = 10f;

    public bool IA = false;

    public virtual void Initialize(int playerIndex, bool ia)
    {
        PlayerIndex = playerIndex;
        IA = ia;
    }
}
