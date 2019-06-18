using System.Collections.Generic;
using UnityEngine;

public partial class Consts {

    public ControllerSO ControllerSO;

}

/// <summary>
/// This class controlls a "state machine" of a character
/// </summary>
public class Controller : MonoBehaviour {

    #region [Consts]

    public ControllerSO CONTROLLER_SO;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private Transform f_mirror;

    [SerializeField]
    protected SpriteRenderer f_spriteRenderer;

    [SerializeField]
    private float f_height;

    [SerializeField]
    private float f_halfWidth;

    [SerializeField]
    private bool f_needsGroundMovement;

    [SerializeField]
    private bool f_lookAtPlayer;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Animator f_animator;

    [SerializeField, Autohook]
    protected Health f_health;

    private GroundMovementRaycast f_groundMovement;
    private AirMovement f_airMovement;
    private Movement f_movement;

    #endregion

    #region [PrivateVariables]

    protected ControllerState m_activeState;
    protected ControllerState m_activeStackedState;

    //private ControllerState m_lastState;
    private int m_stateStartedFrame;
    private int m_stateStartedStackedFrame;

    #endregion

    #region [Properties]

    public ControllerState ActiveState { get { return m_activeState; } }

    public Animator Animator { get { return f_animator; } }
    public SpriteRenderer SpriteRenderer { get { return f_spriteRenderer; } }
    public float Height { get { return f_height; } }
    public float HalfWidth { get { return f_halfWidth * 0.75f; } }

    public Vector2 Velocity { get; set; }
    public bool Backwards { get; set; }
    public bool Grounded { get; set; }

    // This does not work
    //public Vector2 LookDirection { get { return f_mirror.right; } }

    //public ControllerState LastState { get { return m_lastState; } }
    public int StateStartedFrame { get { return m_stateStartedFrame; } }
    public int StateStartedStackedFrame { get { return m_stateStartedStackedFrame; } }
    public GroundMovementRaycast GroundMovement { get { return f_groundMovement; } }
    public AirMovement AirMovement { get { return f_airMovement; } }
    public Movement Movement { get { return f_movement; } }
    public Health Health { get { return f_health; } }

    #endregion

    #region [Init]

    protected virtual void Awake() {
        if (f_needsGroundMovement) {
            f_groundMovement = new GroundMovementRaycast(this, f_height, HalfWidth, this is Player);
            f_movement = f_groundMovement;
        } else {
            f_airMovement = new AirMovement(this);
            f_movement = f_airMovement;
        }
    }

    #endregion

    #region [Updates]

    protected virtual void FixedUpdate() {

        // check collisions
        // check the active collisions
        // ?

        if (m_activeStackedState != null) {
            InputManager.Instance.IgnoreInput = m_activeStackedState.ConsumesInputAndEffects;
        }

        foreach (ControllerState state in m_activeState.FutureStates) {
            bool entered = state.EnterOnCondition(/*m_activeState*/);
            if (entered) {
                m_activeState.Abort();
                state.LogicalEnter();

                if (m_activeStackedState == null || !m_activeStackedState.ConsumesInputAndEffects) {
                    state.EffectualEnter();
                }

                //m_lastState = m_activeState;
                m_stateStartedFrame = GameManager.Instance.Frame;

                Debug.Log("New active: " + state);
                m_activeState = state;
                break;
            }
        }

        if (m_activeStackedState != null) {
            foreach (ControllerState state in m_activeStackedState.StackedStates) {
                if (state.EnterOnCondition()) {
                    m_activeStackedState?.Abort();
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
                    m_activeStackedState?.Abort();
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
                m_activeStackedState.Abort();
                m_activeStackedState = null;
                m_activeState.EffectualEnter();
            }
        }

        //TODO; For hold effects in special states; that won't work now
        //T** Therefore I should define better when stuff is blocked (maybe only in m_activeStates)
        InputManager.Instance.IgnoreInput = false;

        Move();
    }

    #endregion

    #region [PrivateMethods]

    protected virtual void Move() {
        // moving
        {
            Vector2 origin;
            float boxHeight;
            //LayerMask GROUND_MASK = LayerMask.GetMask("Default");
            LayerMask GROUND_MASK = f_movement.Mask;

            // On the ground, the feet will be ignored, since they often walk through the floor a bit
            if (Grounded) {
                origin = transform.TransformPoint(new Vector2(0, f_height * 0.75f));
                boxHeight = f_height / 2f * transform.localScale.y;
            } else { // in air
                origin = transform.TransformPoint(new Vector2(0, f_height * 0.55f));
                boxHeight = f_height * 0.9f * transform.localScale.y;

                //if (Vector2.Angle(transform.up, Velocity) > 45) {
                if (Velocity.y <= 0 || f_movement is AirMovement) {
                    boxHeight *= 0.9f;
                }
            }

            RaycastHit2D hit;

            // Trying to move
            // When there is a collision, the character only gets moved a bit and the velocity changes
            // It is then tried again if the character can move
            for (int i = 0; i < CONTROLLER_SO.MAX_COLLISION_ITERATIONS; ++i) {

                hit = Physics2D.BoxCast(origin, new Vector2(HalfWidth * 2f - 0.05f, boxHeight - 0.05f), transform.eulerAngles.z, Velocity, Velocity.magnitude / 60f, GROUND_MASK);

                if (hit) {

                    if (hit.fraction == 0) {
                        // TODO; not sure about this one
                        // necessary but does some stupid stuff
                        GroundMovement?.TryStickToGround(true);
                        //transform.position = transform.position + (Vector3)(hit.normal * 0.1f + Velocity.normalized * 0.05f);
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

        Orient();

        f_animator.SetFloat("X", Velocity.x);
        f_animator.SetFloat("Y", Velocity.y);

    }

    #endregion

    #region [PublicMethods]

    public void Orient() {
        // Setting x scale (mirroring)
        if (Velocity.sqrMagnitude > 0) {
            float right = System.Math.Sign(Vector2.Dot(Velocity, transform.right));

            if (right != 0) {
                right *= Backwards ? -1 : 1;
                f_mirror.localScale = new Vector3(right, 1, 1);
            }

        } else if (f_lookAtPlayer) {
            Vector3 transformedPlayer = transform.InverseTransformPoint(Consts.Instance.Player.transform.position);
            if (transformedPlayer.x > 0) {
                f_mirror.localScale = new Vector3(1, 1, 1);
            } else if (transformedPlayer.x < 0) {
                f_mirror.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void SetStartState(ControllerState state) {
        if (m_activeStackedState) {
            m_activeStackedState.Abort();
            m_activeStackedState = null;
        }

        if (m_activeState) {
            m_activeState.Abort();
        }

        m_activeState = state;
        m_activeState.LogicalEnter();
        m_activeState.EffectualEnter();
    }

    public void ReactOnImpact(Vector2 hitNormal, bool shorter = false) {

        if (m_activeStackedState) {
            m_activeStackedState.Abort();
            m_activeStackedState = null;
            m_activeState.EffectualEnter();
        }

        // TODO I commented that whole thing out
        //TODO; I might have to set backwards
        //T** However,the normal states set backwards themselves, so it would be weird
        //For now I just don't change anything at all and just abort the current specialstate

        //Vector2 dummyVelocity = Velocity;

        //TODO; this is because the scalled monsters somewhere have wrong values
        hitNormal.y = 0;

        Velocity = hitNormal * CONTROLLER_SO.IMPACT_LENGTH;

        // if (shorter) {
        //     Velocity *= 0.5f;
        // }

        Move();
        //Velocity = dummyVelocity;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmos() {
        f_movement?.OnDrawGizmos();
    }

#endif

}