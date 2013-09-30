using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Animation
{
    public string Name;
    public int StartX;
    public int StartY;
    public int FramesNumber;
    public bool Loop;
    public float Rate;

    public Vector2 SpritesheetDirection;
}

/// <summary>
/// Animation script helper
/// </summary>
public class AnimationScript : MonoBehaviour
{
    public List<Animation> Animations;

    private Animation CurrentAnimation;
    private bool mIsAnimated;
    private float mAnimationCooldown = 0f;
    private int mFrameIndex = 0;
    private int mAnimationLoopCount = 0;

    void Awake()
    {
    }

    void Update()
    {
        if (mIsAnimated)
        {
            bool updateSprite = false;
            mAnimationCooldown -= GameTimeScript.DeltaTime;

            if (mAnimationCooldown <= 0)
            {
                mAnimationCooldown = CurrentAnimation.Rate;
                mFrameIndex++;
                updateSprite = true;

                if (mFrameIndex >= CurrentAnimation.FramesNumber)
                {
                    if (CurrentAnimation.Loop)
                    {
                        // Loop
                        mFrameIndex = 0;
                        mAnimationLoopCount++;
                    }
                    else
                    {
                        // End
                        CurrentAnimation = null;
                        mIsAnimated = false;
                        updateSprite = false;

                        CurrentAnimationEnded = true;
                    }
                }
            }

            if (updateSprite)
            {
                // Change materials UV
                renderer.material.mainTextureOffset = new Vector2(
                    CurrentAnimation.StartX + (mFrameIndex * CurrentAnimation.SpritesheetDirection.x * renderer.material.mainTextureScale.x),
                    CurrentAnimation.StartY + (mFrameIndex * CurrentAnimation.SpritesheetDirection.y * renderer.material.mainTextureScale.y)
                );
            }
        }
    }

    /// <summary>
    /// Launch an animation with the given name
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        Debug.Log("Animation "+name);

        // Find the requested animation
        foreach (var anim in Animations)
        {
            if (anim.Name.ToLower() == name.ToLower())
            {
                CurrentAnimation = anim;
                break;
            }
        }

        if (CurrentAnimation == null)
        {
            Debug.LogError("Animation named \"" + name + "\" wasn't found for " + gameObject);
        }
        else
        {
            mAnimationCooldown = 0f;
            mFrameIndex = 0;
            mAnimationLoopCount = 0;

            mIsAnimated = true;
            CurrentAnimationEnded = false;
        }
    }

    public bool IsPlaying(string name)
    {
        if (mIsAnimated && CurrentAnimation != null)
        {
            return (CurrentAnimation.Name.ToLower() == name.ToLower());
        }

        return false;
    }

    /// <summary>
    /// Stop the current animation
    /// </summary>
    public void Stop()
    {
        if (mIsAnimated && CurrentAnimation != null)
        {
            CurrentAnimation = null;
            mIsAnimated = false;
        }
    }

    public bool CurrentAnimationEnded { get; private set; }
}
