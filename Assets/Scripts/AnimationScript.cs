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
    private float mAnimationCooldown = 0f;
    private int mFrameIndex = 0;
    private int mAnimationLoopCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (CurrentAnimation != null)
        {
            bool updateSprite = false;
            mAnimationCooldown -= Time.deltaTime;

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
                        updateSprite = false;
                    }
                }
            }

            if (updateSprite)
            {
                // Change materials UV
                // TODO
                //renderer.material.mainTextureOffset = new Vector2(x * renderer.material.mainTextureScale.x, z * renderer.material.mainTextureScale.y);
            }
        }
    }

    /// <summary>
    /// Launch an animation with the given name
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
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


        }
    }
}
