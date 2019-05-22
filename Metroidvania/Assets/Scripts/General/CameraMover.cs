using System;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    private const float CAMERA_TIME = 2;
    private const float MAX_SPEED = 5;

    #region [PrivateVariables]

    private Vector2 m_target;
    private Action m_callback;

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        Vector3 goal = Vector2.SmoothDamp(transform.position, m_target, ref Consts.Instance.Camera.Velocity, CAMERA_TIME, MAX_SPEED);
        goal.z = -10;
        transform.position = goal;

        if (((Vector2)transform.position).Equals(m_target)) {
            enabled = false;
            m_callback();
        }

    }

    #endregion

    #region [PublicMethods]

    public void MoveCamera(Vector2 target, Action callback) {
        //The caller now has to do that manually:
        //Consts.Instance.Camera.FollowCam.enabled = false;

        m_target = target;
        m_callback = callback;

        // This is as long as the FollowCam does not use SmoothDamp
        Consts.Instance.Camera.Velocity = Vector2.zero;

        enabled = true;
    }

    #endregion

}