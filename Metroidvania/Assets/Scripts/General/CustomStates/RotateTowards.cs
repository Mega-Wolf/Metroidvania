using System;
using UnityEngine;
using WolfBT;

public class RotateTowardsValue : BTState {

    #region [FinalVariables]

    private Transform f_transform;
    private float f_endRotation;
    private float f_maxSpeed;
    private ReferenceFrame f_referenceFrame;

    #endregion

    #region [Constructors]

    public RotateTowardsValue(Transform transform, float endRotation, float maxSpeed, ReferenceFrame referenceFrame = ReferenceFrame.Local) {
        f_transform = transform;
        f_endRotation = endRotation;
        f_maxSpeed = maxSpeed;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() { }

    public override BTStateReturn FixedUpdate(int frames) {
        if (f_referenceFrame == ReferenceFrame.Local) {
            float angle = Mathf.MoveTowardsAngle(f_transform.localRotation.eulerAngles.z, f_endRotation, f_maxSpeed * frames);
            f_transform.localRotation = Quaternion.Euler(0, 0, angle);
            if (Mathf.Approximately(angle, f_endRotation)) {
                return BTStateReturn.True;
            }
        } else {
            float angle = Mathf.MoveTowardsAngle(f_transform.rotation.eulerAngles.z, f_endRotation, f_maxSpeed * frames);
            f_transform.rotation = Quaternion.Euler(0, 0, angle);
            if (Mathf.Approximately(angle, f_endRotation)) {
                return BTStateReturn.True;
            }
        }

        return BTStateReturn.Running;
    }

    #endregion
}

public class RotateTowardsTransform : BTState {

    #region [FinalVariables]

    private Transform f_transform;
    private Transform f_target;
    private float f_maxSpeed;
    private ReferenceFrame f_referenceFrame;

    #endregion

    #region [Constructors]

    public RotateTowardsTransform(Transform transform, Transform target, float maxSpeed, ReferenceFrame referenceFrame = ReferenceFrame.Local) {
        f_transform = transform;
        f_target = target;
        f_maxSpeed = maxSpeed;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() { }

    public override BTStateReturn FixedUpdate(int frames) {
        Vector2 direction = f_target.position - f_transform.position;
        Quaternion lookRotation = Quaternion.FromToRotation(Vector2.up, direction);

        Quaternion rot;

        if (f_referenceFrame == ReferenceFrame.Local) {
            rot = Quaternion.RotateTowards(f_transform.localRotation, lookRotation, frames * f_maxSpeed);
        } else {
            rot = Quaternion.RotateTowards(f_transform.rotation, lookRotation, frames * f_maxSpeed);
        }

        f_transform.localRotation = rot;
        if (rot.eulerAngles == lookRotation.eulerAngles) {
            return BTStateReturn.True;
        }

        return BTStateReturn.Running;
    }

    #endregion
}