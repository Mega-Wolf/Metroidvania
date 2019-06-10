using WolfBT;
using System;
using UnityEngine;

/// <summary>
/// This node runs an animation and waits for the specified amount of frames
/// </summary>
public static class Actions {

    public static Action PlayAnimation(Animator animator, string name) {
        return () => {
            animator.Play(name);
        };
    }

    public static Action LookAt(Transform transform, Transform target) {
        return () => {
            transform.rotation = Quaternion.FromToRotation(Vector2.up, target.position - transform.position);
        };
    }

}