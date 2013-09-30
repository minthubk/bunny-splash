using UnityEngine;
using System.Collections;

public class EatHuntPlayerScript : PlayerScript
{
    public float Speed = 10f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);

        Vector3 movement = new Vector3(
            x * Speed * GameTimeScript.DeltaTime,
            y * Speed * GameTimeScript.DeltaTime,
            0);

        // Add inertia
        // But limit to 5
        if (Mathf.Abs(rigidbody.velocity.x) < 5)
        {
            rigidbody.AddForce(movement * 100);
        }
        rigidbody.MovePosition(rigidbody.position + movement);
    }
}
