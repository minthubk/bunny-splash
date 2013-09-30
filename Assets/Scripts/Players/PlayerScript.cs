using UnityEngine;
using System.Collections;

public abstract class PlayerScript : MonoBehaviour
{
    public int PlayerIndex = 1;

    public virtual void Initialize(int playerIndex)
    {
        PlayerIndex = playerIndex;
    }
}
