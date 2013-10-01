using UnityEngine;
using System.Collections;

public class HunterPlayerScript : PlayerScript 
{
    void Update()
    {
        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);

        Vector3 movement = new Vector3(
            x * Speed * GameTimeScript.DeltaTime,
            y * Speed * GameTimeScript.DeltaTime,
            0);

        transform.Translate(movement);
    }

    void OnTriggerEnter(Collider collider)
    {
        // Coin?
        CoinInFogScript coin = collider.gameObject.GetComponent<CoinInFogScript>();
        if (coin != null)
        {
            coin.SetIlluminated(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Hunter?
        CoinInFogScript coin = collider.gameObject.GetComponent<CoinInFogScript>();
        if (coin != null)
        {
            coin.SetIlluminated(false);
        }
    }
}
