using UnityEngine;
using static GroundMovementRaycast;
/// <summary>
/// Walking around as a monster like a Goomba
/// </summary>
public class GoombaWalk : ControllerState {

    #region [Consts]

    [SerializeField]
    private FloatSO SPEED;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private bool m_stopAtEdges = true;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        if (f_controller.Velocity.y > 0) {
            return false;
        }

        // The following checks if the monster touches the ground
        return f_controller.GroundMovement.TryStickToGround(true);
    }

    public override void LogicalEnter() {
        //f_controller.Grounded = true;
        f_controller.Backwards = false;

        //TODO; does not care about transform
        if (f_controller.Velocity.x < -0.1f) {
            f_controller.GroundMovement.MovingRight = false;
        } else if (f_controller.Velocity.x > 0.1f) {
            f_controller.GroundMovement.MovingRight = true;
        }
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
            f_controller.GroundMovement.MovingRight = !f_controller.GroundMovement.MovingRight;
        }



        f_controller.Velocity = Vector2.zero;

        GroundTouch gt = f_controller.GroundMovement.Move((f_controller.GroundMovement.MovingRight ? 1 : -1) * SPEED * BossFightRhino.SPEED);

        if (m_stopAtEdges) {
            int airDirection = GroundMovementRaycast.AirDirection(gt);
            if (airDirection < 0) {
                f_controller.GroundMovement.MovingRight = true;
            } else if (airDirection > 0) {
                f_controller.GroundMovement.MovingRight = false;
            }
        }

        return true;
    }

    public override void Abort() {
        //f_controller.Velocity = Vector2.zero;
    }

    #endregion

}