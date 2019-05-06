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
    private float f_height;

    [SerializeField]
    private float f_halfWidth;

    #endregion

    #region [PrivateVariables]

    private ControllerState m_activeState;

    #endregion

    #region [Properties]

    // collision results

    public Animator Animator { get { return f_animator; } }
    public float Height { get { return f_height; } }
    public float HalfWidth { get { return f_halfWidth * 0.75f; } }

    public Vector2 Velocity { get; set; }

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        // check collisions
        // check the active collisions
        // ?

        foreach (ControllerState state in m_activeState.FutureStates) {
            bool entered = state.EnterOnCondition(m_activeState);
            if (entered) {
                Debug.Log("New active: " + state);
                m_activeState = state;
                return;
            }
        }

        m_activeState.HandleFixedUpdate();

        
        // TODO; check for collisions

        // Update position
        transform.position = transform.position + (Vector3)Velocity / 60f;


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