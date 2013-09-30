using UnityEngine;
using System.Collections;

/// <summary>
/// Set the gravity on Awake for the whole scene
/// </summary>
public class GravityController : MonoBehaviour
{
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);

    void Awake()
    {
        Physics.gravity = Gravity;
    }

}
