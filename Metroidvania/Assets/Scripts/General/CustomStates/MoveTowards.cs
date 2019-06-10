using System;
using UnityEngine;
using WolfBT;

//TODO; a lot of stuff is not working here due to local/global

public class MoveTowardsValue : BTState {

    #region [FinalVariables]

    private Transform f_transform;
    private Func<Vector2> f_endPosition;
    private float f_maxSpeed;
    private ReferenceFrame f_referenceFrame;

    #endregion

    #region [Constructors]

    public MoveTowardsValue(Transform transform, Func<Vector2> endPosition, float maxSpeed, ReferenceFrame referenceFrame = ReferenceFrame.Global) {
        f_transform = transform;
        f_endPosition = endPosition;
        f_maxSpeed = maxSpeed;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() { }

    public override BTStateReturn FixedUpdate(int frames) {
        Vector2 pos;

        Vector2 endPosition = f_endPosition();

        if (f_referenceFrame == ReferenceFrame.Local) {
            pos = Vector2.MoveTowards(f_transform.localPosition, endPosition, frames * f_maxSpeed);
            f_transform.localPosition = pos;
        } else {
            pos = Vector2.MoveTowards(f_transform.position, endPosition, frames * f_maxSpeed);
            f_transform.position = pos;
        }

        if (pos == endPosition) {
            return BTStateReturn.True;
        }

        return BTStateReturn.Running;
    }

    #endregion
}

public class MoveTowardsTransform : BTState {

    #region [FinalVariables]

    private Transform f_transform;
    private Func<Transform> f_target;
    private float f_maxSpeed;
    private ReferenceFrame f_referenceFrame;

    #endregion

    #region [Constructors]

    public MoveTowardsTransform(Transform transform, Func<Transform> target, float maxSpeed, ReferenceFrame referenceFrame = ReferenceFrame.Global) {
        f_transform = transform;
        f_target = target;
        f_maxSpeed = maxSpeed;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() { }

    public override BTStateReturn FixedUpdate(int frames) {
        Vector2 pos;

        Vector2 position = f_target().position;

        if (f_referenceFrame == ReferenceFrame.Local) {
            pos = Vector2.MoveTowards(f_transform.localPosition, position, frames * f_maxSpeed);
            f_transform.localPosition = pos;
        } else {
            pos = Vector2.MoveTowards(f_transform.position, position, frames * f_maxSpeed);
            f_transform.position = pos;
        }

        if (pos == position) {
            return BTStateReturn.True;
        }

        return BTStateReturn.Running;
    }

    #endregion
}