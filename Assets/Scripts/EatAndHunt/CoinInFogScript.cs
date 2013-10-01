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
            StartCoroutine(FadeOut());
        }
    }

    /// <summary>
    /// Eat by something
    /// </summary>
    /// <param name="eater"></param>
    public void EatBy(EaterPlayerScript eater)
    {
        if (IsAte == false)
        {
            // Play sound
            SoundBankScript.Instance.Play(SoundBankScript.Instance.Eat[Random.Range(0, SoundBankScript.Instance.Eat.Count)]);

            IsAte = true;

            // Kill too?
            if (IsAte && IsIlluminated)
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    /// <summary>
    /// Disappear with animation
    /// </summary>
    private IEnumerator FadeOut()
    {
        // Fade out animation
        // TODO

        GameObject.DestroyObject(gameObject);
        yield return null;
    }
}
