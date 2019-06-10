using WolfBT;
using UnityEngine;
using System;

public class Move : TimedState {

    #region [FinalVariables]

    private Transform f_transform;
    private AnimationCurve f_animationCurve;
    private Func<Vector2> f_goal;

    private ReferenceFrame f_referenceFrame;

    private Vector2 f_startPosition;

    #endregion

    #region [Constructors]

    public Move(Transform transform, AnimationCurve animationCurve, Func<Vector2> goal, ReferenceFrame referenceFrame = ReferenceFrame.Global) {
        f_transform = transform;
        f_animationCurve = animationCurve;
        f_goal = goal;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() {
        if (f_referenceFrame == ReferenceFrame.Local) {
            f_startPosition = f_transform.localPosition;
        } else {
            f_startPosition = f_transform.position;
        }
    }

    public override BTStateReturn UpdateToPercentage(float percentage) {
        if (f_referenceFrame == ReferenceFrame.Local) {
            f_transform.localPosition = Vector2.Lerp(f_startPosition, f_goal(), f_animationCurve.Evaluate(percentage));
        } else {
            f_transform.position = Vector2.Lerp(f_startPosition, f_goal(), f_animationCurve.Evaluate(percentage));
        }

        return BTStateReturn.Running;
    }

    #endregion

}