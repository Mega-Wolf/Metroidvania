using BehaviourTree;
using UnityEngine;

/// <summary>
/// This node runs an animation and waits for the specified amount of frames
/// </summary>
public class PlayAnimation : BTState {

    #region [FinalVariables]

    private readonly Animator f_animator;
    private readonly string f_name;
    private readonly int f_maxFrames;

    #endregion

    #region [PrivateVariables]

    private int m_currentFrame;

    #endregion

    #region [Constructors]

    public PlayAnimation(Animator animator, string name, int maxFrames) {
        f_animator = animator;
        f_name = name;
        f_maxFrames = maxFrames;
    }

    #endregion

    #region [Override]

    public override void Enter() {
        m_currentFrame = 0;
        f_animator.Play(f_name);
    }

    public override BTStateReturn FixedUpdate(int frames) {
        if (m_currentFrame >= f_maxFrames) {
            return BTStateReturn.True;
        }

        m_currentFrame += frames;

        return BTStateReturn.Running;
    }

    #endregion

}