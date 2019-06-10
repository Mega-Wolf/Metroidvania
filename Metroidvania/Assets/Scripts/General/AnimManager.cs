// using System.Collections.Generic;
// using UnityEngine;

// public class AnimManager : MonoBehaviour {

//     #region [FinalVariables]

//     private HashSet<Anim> f_anims = new HashSet<Anim>();

//     #endregion

//     #region [Updates]

//     private void FixedUpdate() {
//         HashSet<Anim> deletes = new HashSet<Anim>();

//         foreach (Anim anim in f_anims) {
//             bool finished = anim.UpdateCheckFinished();
//             if (finished) {
//                 deletes.Add(anim);
//             }
//         }

//         f_anims.ExceptWith(deletes);
//     }

//     #endregion

//     #region [PublicMethods]

//     public void RegisterAnimation(Anim anim) {
//         f_anims.Add(anim);
//     }

//     public void DeregisterAnimation(Anim anim) {
//         f_anims.Remove(anim);
//     }

//     #endregion

// }