using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowCam : MonoBehaviour {

    private const float CAMERA_TIME = 0.1f;
    private const float MAX_SPEED = 100;

    #region [MemberFields]

    [SerializeField]
    private Transform m_followed;
    public Transform Followed { set { m_followed = value; } }

    #endregion

    #region [Updates]

    private void LateUpdate() {
        //transform.position = m_followed.position - 10 * Vector3.forward;
        //transform.position = m_followed.position - 10 * Vector3.forward + 3 * Vector3.up;

        Vector3 goal = Vector2.SmoothDamp(transform.position, m_followed.position + 3 * Vector3.up, ref Consts.Instance.Camera.Velocity, CAMERA_TIME, MAX_SPEED);
        goal.z = -10;
        transform.position = goal;
    }

    #endregion
}
