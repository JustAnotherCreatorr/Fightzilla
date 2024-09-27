using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimLength 
{
    public static float GetAnimLength(Animator animator, string animName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
            {
                return clip.length;
            }
        }

        Debug.LogError("no clip matches that name");
        return 0;
    }
}
