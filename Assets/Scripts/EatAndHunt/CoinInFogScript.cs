using UnityEngine;
using System.Collections;

public class CoinInFogScript : MonoBehaviour
{
    public bool IsAte { get; private set; }

    public bool IsIlluminated { get; private set; }

    void Start()
    {
        IsAte = false;
        IsIlluminated = false;
    }

    /// <summary>
    /// Illuminated by something
    /// </summary>
    /// <param name="ill"></param>
    public void SetIlluminated(bool ill)
    {
        IsIlluminated = ill;

        if (IsAte && IsIlluminated)
        {
            FadeOut();
        }
    }

    /// <summary>
    /// Eat by something
    /// </summary>
    /// <param name="eater"></param>
    public void EatBy(EaterPlayerScript eater)
    {
        // Play sound
        SoundBankScript.Instance.Play(SoundBankScript.Instance.Eat[Random.Range(0, SoundBankScript.Instance.Eat.Count)]);

        IsAte = true;

        // Kill too?
        if (IsAte && IsIlluminated)
        {
            FadeOut();
        }
    }

    /// <summary>
    /// Disappear with animation
    /// </summary>
    private void FadeOut()
    {
        GameObject.DestroyObject(gameObject);
    }
}
