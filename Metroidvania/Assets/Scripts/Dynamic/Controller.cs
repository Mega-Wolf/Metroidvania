using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controlls a "state machine" of a character
/// </summary>
[RequireComponent(typeof(Animator))]
public class Controller : MonoBehaviour {

    #region [Consts]

    private const int MAX_COLLISION_ITERATIONS = 3;

    #endregion

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
    private ControllerState m_activeStackedState;

    //private ControllerState m_lastState;
    private int m_stateStartedFrame;
    private int m_stateStartedStackedFrame;

    #endregion

    #region [Properties]

    // collision results

    public Animator Animator { get { return f_animator; } }
    public float Height { get { return f_height; } }
    public float HalfWidth { get { return f_halfWidth * 0.75f; } }

    public Vector2 Velocity { get; set; }
    public bool Backwards { get; set; }
    public bool Grounded { get; set; }

    //public ControllerState LastState { get { return m_lastState; } }
    public int StateStartedFrame { get { return m_stateStartedFrame; } }
    public int StateStartedStackedFrame { get { return m_stateStartedStackedFrame; } }

    #endregion

    #region [Updates]

    private void FixedUpdate() {

        // check collisions
        // check the active collisions
        // ?

        if (m_activeStackedState != null) {
            InputManager.Instance.IgnoreInput = m_activeStackedState.ConsumesInput;
        }

        foreach (ControllerState state in m_activeState.FutureStates) {
            bool entered = state.EnterOnCondition(/*m_activeState*/);
            if (entered) {
                state.LogicalEnter();
                state.EffectualEnter();

                //m_lastState = m_activeState;
                m_stateStartedFrame = GameManager.Instance.Frame;

                Debug.Log("New active: " + state);
                m_activeState = state;
                break;
            }
        }

        InputManager.Instance.IgnoreInput = false;

        if (m_activeStackedState != null) {
            foreach (ControllerState state in m_activeStackedState.StackedStates) {
                if (state.EnterOnCondition()) {
                    state.LogicalEnter();
                    state.EffectualEnter();

                    m_stateStartedStackedFrame = GameManager.Instance.Frame;
                    m_activeStackedState = state;
                    break;
                }
            }
        } else {
            //DUNNO; Should the active state be able to still trigger stacked states?
            //DUNNO; Should I be able to add non stacked states to a stacked state

            foreach (ControllerState state in m_activeState.StackedStates) {
                if (state.EnterOnCondition()) {
                    state.LogicalEnter();
                    state.EffectualEnter();

                    m_stateStartedStackedFrame = GameManager.Instance.Frame;
                    m_activeStackedState = state;
                    break;
                }
            }
        }

        m_activeState.HandleFixedUpdate();

        if (m_activeStackedState != null) {
            bool keep = m_activeStackedState.HandleFixedUpdate();
            if (!keep) {
                m_activeStackedState = null;
            }
        }

        // moving
        {
            Vector2 origin;
            float boxHeight;
            LayerMask GROUND_MASK = LayerMask.GetMask("Default");

            // On the ground, the feet will be ignored, since they often walk through the floor a bit
            if (Grounded) {
                origin = transform.TransformPoint(new Vector2(0, f_height * 0.75f));
                boxHeight = f_height / 2f;
            } else { // in air
                origin = transform.TransformPoint(new Vector2(0, f_height * 0.55f));
                boxHeight = f_height * 0.9f;

                //if (Vector2.Angle(transform.up, Velocity) > 45) {
                if (Velocity.y <= 0) {
                    boxHeight *= 0.9f;
                }
            }

            RaycastHit2D hit;

            // Trying to move
            // When there is a collision, the character only gets moved a bit and the velocity changes
            // It is then tried again if the character can move
            for (int i = 0; i < MAX_COLLISION_ITERATIONS; ++i) {

                hit = Physics2D.BoxCast(origin, new Vector2(HalfWidth * 2f - 0.05f, boxHeight - 0.05f), transform.eulerAngles.z, Velocity, Velocity.magnitude / 60f, GROUND_MASK);

                if (hit) {

                    if (hit.fraction == 0) {
                        // TODO; not sure about this one
                        transform.position = transform.position + (Vector3)(hit.normal * 0.1f + Velocity.normalized * 0.05f);

                    } else {
                        // updating the position by the fraction of the velocity which worked
                        transform.position = transform.position + (hit.fraction - 0.1f) / 60f * (Vector3)Velocity;
                    }



                    //Vector2 parallel = Vector2.Perpendicular(hit.normal);
                    Vector2 parallel = Vector3.Cross(hit.normal, Vector3.forward);

                    float parFraction = Vector2.Dot((1 - hit.fraction + 0.1f) / 60f * Velocity, parallel);

                    // this cancels out the Velocity in which the character is moving at the moment
                    Velocity = (60f * parFraction * parallel);

                    // the character will NOT move the rest of the movement here anymore; this will happen in the next iteration step
                } else {

                    // moving normally
                    transform.position = transform.position + (Vector3)Velocity / 60f;

                    // Since the velocity has not been updated, this process can be ended
                    break;
                }
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