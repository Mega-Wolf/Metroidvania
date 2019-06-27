using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowCam : MonoBehaviour {

    #region [MemberFields]

    [SerializeField]
    private Transform m_followed;
    public Transform Followed { set { m_followed = value; } }

    #endregion

    #region [Updates]

    private void LateUpdate() {
        //transform.position = m_followed.position - 10 * Vector3.forward;
        transform.position = m_followed.position - 10 * Vector3.forward + 3 * Vector3.up;

        //Vector3 goal = Vector2.SmoothDamp(transform.position, m_followed.position, ref Consts.Instance.Camera.Velocity, CAMERA_TIME, MAX_SPEED);
        //goal.z = -10;
        //transform.position = goal;
    }

    #endregion
}
