using System;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    private const float CAMERA_TIME = 1;
    private const float MAX_SPEED = 10;

    #region [PrivateVariables]

    private Vector2 m_target;
    private Action m_callback;

    private int m_currentFrames = 0;

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        float percentage = m_currentFrames / (50 * CAMERA_TIME);

        Vector2 target = Vector2.Lerp(Consts.Instance.Player.transform.position, m_target, percentage);

        if (m_currentFrames < CAMERA_TIME * 50) {
            ++m_currentFrames;
        }

        Vector3 goal = Vector2.SmoothDamp(transform.position, target, ref Consts.Instance.Camera.Velocity, CAMERA_TIME, MAX_SPEED);
        goal.z = -10;
        transform.position = goal;

        if (Vector2.Distance(transform.position, m_target) < 0.05f && m_currentFrames >= CAMERA_TIME * 50) {
            transform.position = new Vector3(m_target.x, m_target.y, -10);
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