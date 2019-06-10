// using UnityEngine;

// public abstract class Anim {

//     #region [FinalVariables]

//     //private int f_startFrame;
//     private int f_durationFrames;

//     protected Transform f_transform;
//     protected AnimationCurve f_animationCurve;

//     #endregion

//     #region [PrivateVariables]

//     protected int m_currentFrames;

//     #endregion

//     #region [Constructors]

//     public Anim(int durationFrames, Transform transform, AnimationCurve animationCurve) {
//         //f_startFrame = GameManager.Instance.Frame;
//         f_durationFrames = durationFrames;
//         f_transform = transform;
//         f_animationCurve = animationCurve;

//         m_currentFrames = 0;
//     }

//     #endregion

//     #region [PublicMethods]

//     public bool UpdateCheckFinished() {
//         ++m_currentFrames;

//         if (m_currentFrames == f_durationFrames) {
//             return true;
//         }

//         return false;
//     }

//     #endregion

//     #region [PrivateVariables]

//     protected abstract void Step();

//     #endregion

// }

// // public class AnimTowards : Anim {

// //     public Transform f_transform = null;
// //     public Vector2 f_centre = Vector2.zero;

// // }

// public class AnimDir : Anim {

//     #region [Constructor]

//     public AnimDir(int durationFrames, Transform transform, AnimationCurve animationCurve) : base(durationFrames, transform, animationCurve) {
//     }

//     protected override void Step() {
//         throw new System.NotImplementedException();
//     }

// }

// public class AnimRot : Anim {

//     #region [FinalVariables]

//     private Transform f_centreTrans;
//     private Vector2 f_centre;

//     #endregion

//     #region [Constructors]

//     public AnimRot(int durationFrames, Transform transform, AnimationCurve animationCurve, Transform goal) : base(durationFrames, transform, animationCurve) {
//         f_centreTrans = goal;
//     }

//     public AnimRot(int durationFrames, Transform transform, AnimationCurve animationCurve, Vector2 goal) : base(durationFrames, transform, animationCurve) {
//         f_centre = goal;
//     }

//     #endregion

//     #region [Override]

//     protected override void Step() {
//         f_transform.
//     }

//     #endregion

// }