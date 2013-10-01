using System;
using System.Linq;
using UnityEngine;
using System.Collections;

/// <summary>
/// Easy to use timers
/// </summary>
public static class Timers
{

    /// <summary>
    /// Simple timer, wait and then execute something
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator Start(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);

        if (callback != null) callback();
    }


    /// <summary>
    /// Advanced timer, executing code at each step
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="stepDuration"></param>
    /// /// <param name="stepCallback"
    /// <param name="completedCallback"></param>
    /// <returns></returns>
    public static IEnumerator Start(float duration, float stepDuration, Action<float> stepCallback, Action<float> completedCallback)
    {
        // How many steps?
        int steps = (int)Math.Ceiling(duration / stepDuration);

        for (int i = 0; i < steps; i++)
        {
            if (stepCallback != null) stepCallback(i * stepDuration);

            yield return new WaitForSeconds(stepDuration);
        }

        if (completedCallback != null) completedCallback(duration);
    }


}
