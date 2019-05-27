using UnityEngine;
using static GroundMovementRaycast;
/// <summary>
/// Walking around as a monster like a Goomba
/// </summary>
public class GoombaWalk : ControllerState {

    #region [Speed]

    [SerializeField]
    private FloatSO SPEED;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private bool m_stopAtEdges = true;

    #endregion

    #region [PrivateVariables]

    private bool m_walkingRight;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        if (f_controller.Velocity.y > 0) {
            return false;
        }

        // The following checks if the monster touches the ground
        return f_controller.GroundMovement.TryStickToGround();
    }

    public override void LogicalEnter() {
        m_walkingRight = f_controller.Velocity.x > 0;

        //f_controller.Grounded = true;
        f_controller.Backwards = false;
    }

    public override void EffectualEnter() {
        f_controller.Animator.Play("Goomba");
    }

    public override bool HandleFixedUpdate() {

        //TODO; this is such a hack
        if (InputManager.Instance.IgnoreInput) {
            return true;
        }

        if (!f_controller.Grounded) {
            return true;
        }

        if (f_controller.Backwards) {
            f_controller.Backwards = false;
        }
        
        if (Mathf.Abs(f_controller.Velocity.x) < 0.1f) {
            m_walkingRight = !m_walkingRight;
        }

        f_controller.Velocity = Vector2.zero;

        GroundTouch gt = f_controller.GroundMovement.Move((m_walkingRight ? 1 : -1) * SPEED);

        if (m_stopAtEdges) {
            int airDirection = GroundMovementRaycast.AirDirection(gt);
            if (airDirection < 0) {
                m_walkingRight = true;
            } else if (airDirection > 0) {
                m_walkingRight = false;
            }
        }

        return true;
    }

    public override void Abort() {
        //f_controller.Velocity = Vector2.zero;
    }

    #endregion

}