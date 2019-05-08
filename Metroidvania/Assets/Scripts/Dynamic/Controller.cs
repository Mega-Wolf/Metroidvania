using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controlls a "state machine" of a character
/// </summary>
[RequireComponent(typeof(Animator))]
public class Controller : MonoBehaviour {

    #region [MemberFields]

    [SerializeField, Autohook]
    private Animator f_animator;

    [SerializeField]
    private Transform f_visuals;

    [SerializeField]
    private float f_height;

    [SerializeField]
    private float f_halfWidth;

    #endregion

    #region [PrivateVariables]

    private ControllerState m_activeState;

    //private ControllerState m_lastState;
    private int m_stateStartedFrame;

    #endregion

    #region [Properties]

    // collision results

    public Animator Animator { get { return f_animator; } }
    public float Height { get { return f_height; } }
    public float HalfWidth { get { return f_halfWidth * 0.75f; } }

    public Vector2 Velocity { get; set; }
    public bool Backwards { get; set; }

    //public ControllerState LastState { get { return m_lastState; } }
    public int StateStartedFrame { get { return m_stateStartedFrame; } }

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        // check collisions
        // check the active collisions
        // ?

        foreach (ControllerState state in m_activeState.FutureStates) {
            bool entered = state.EnterOnCondition(/*m_activeState*/);
            if (entered) {

                //m_lastState = m_activeState;
                m_stateStartedFrame = GameManager.Instance.Frame;

                Debug.Log("New active: " + state);
                m_activeState = state;
                return;
            }
        }

        m_activeState.HandleFixedUpdate();

        // moving
        {
            Vector2 origin = transform.TransformPoint(new Vector2(0, f_height * 0.75f));
            LayerMask GROUND_MASK = LayerMask.GetMask("Default");

            float boxHeight = f_height / 2f;

            //if (Vector2.Angle(transform.up, Velocity) > 45) {
            if (Velocity.y <= 0) {
                boxHeight *= 0.9f;
            }

            RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(HalfWidth * 2f - 0.05f, boxHeight - 0.05f), transform.eulerAngles.z, Velocity, Velocity.magnitude / 60f, GROUND_MASK);

            if (hit) {
                // updating the position by the fraction of the velocity which worked
                transform.position = transform.position + (hit.fraction) / 60f * (Vector3)Velocity;

                //Vector2 parallel = Vector2.Perpendicular(hit.normal);
                Vector2 parallel = Vector3.Cross(hit.normal, Vector3.forward);

                float parFraction = Vector2.Dot((1 - hit.fraction) / 60f * Velocity, parallel);

                // this cancels out the Velocity in which the character is moving at the moment
                Velocity = (60f * parFraction * parallel);

                // updating the rest of the movment
                transform.position = transform.position + parFraction * (Vector3)parallel;
            } else {

                // moving normally
                transform.position = transform.position + (Vector3)Velocity / 60f;
            }

        }

        // Setting x scale (mirroring)
        if (Velocity.sqrMagnitude > 0) {
            float right = System.Math.Sign(Vector2.Dot(Velocity, transform.right));

            if (right != 0) {
                right *= Backwards ? -1 : 1;
                f_visuals.localScale = new Vector3(right, 1, 1);
            }

        }

        f_animator.SetFloat("X", Velocity.x);
        f_animator.SetFloat("Y", Velocity.y);

    }

    #endregion

    #region [PublicMethods]

    public void SetStartState(ControllerState state) {
        m_activeState = state;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmos() {
        m_activeState?.OnDrawGizmos();
    }

#endif

}