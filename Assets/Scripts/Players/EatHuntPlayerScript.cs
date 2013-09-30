using UnityEngine;
using System.Collections;

public class EatHuntPlayerScript : PlayerScript
{
    void Update()
    {
        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);
    }
}
