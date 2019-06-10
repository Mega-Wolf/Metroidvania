using UnityEngine;
using WolfBT;

public class Rotate : TimedState {

    #region [FinalVariables]

    private Transform f_transform;
    private AnimationCurve f_animationCurve;
    private float f_rotationDegrees;
    private ReferenceFrame f_referenceFrame;

    private float f_startRotation;

    #endregion

    #region [Constructors]

    public Rotate(Transform transform, AnimationCurve animationCurve, float rotationDegrees, ReferenceFrame referenceFrame = ReferenceFrame.Local) {
        f_transform = transform;
        f_animationCurve = animationCurve;
        f_rotationDegrees = rotationDegrees;
        f_referenceFrame = referenceFrame;
    }

    #endregion

    #region [Override]

    public override void Enter() {
        if (f_referenceFrame == ReferenceFrame.Local) {
            f_startRotation = f_transform.localRotation.eulerAngles.z;
        } else {
            f_startRotation = f_transform.rotation.eulerAngles.z;
        }
    }

    public override BTStateReturn UpdateToPercentage(float percentage) {
        if (f_referenceFrame == ReferenceFrame.Local) {
            f_transform.localRotation = Quaternion.Euler(0, 0, f_startRotation + f_animationCurve.Evaluate(percentage) * f_rotationDegrees);
        } else {
            f_transform.rotation = Quaternion.Euler(0, 0, f_startRotation + f_animationCurve.Evaluate(percentage) * f_rotationDegrees);
        }

        return BTStateReturn.Running;
    }

    #endregion

}