using UnityEngine;

public class PlayerGrounded : ControllerState {

    #region [Consts]

    [SerializeField]
    private FloatSO SPEED;

    #endregion

// #if UNITY_EDITOR

//     public void OnDrawGizmos() {
//         f_ground?.OnDrawGizmos();
//     }

// #endif

    #region [Override]

    public override void EffectualEnter() {
        f_controller.Backwards = false;
        f_controller.Animator.Play("Landing");
    }

    public override void LogicalEnter() {
        f_controller.Grounded = true;
        f_controller.Velocity = Vector2.zero;
    }

    public override bool EnterOnCondition() {

        if (f_controller.Velocity.y > 0) {
            return false;
        }

        // The following checks if the player touches the ground
        return f_controller.GroundMovement.TryStickToGround(true);
    }

    public override bool HandleFixedUpdate() {

        f_controller.Velocity = Vector2.zero;

        // Horizontal Movement
        {
            int move = 0;
            if (InputManager.Instance.GetButton("Left")) {
                move = -1;
            }
            if (InputManager.Instance.GetButton("Right")) {
                ++move;
            }

            if (move != 0) {
                f_controller.GroundMovement.Move(move * SPEED);
            }

        }

        return true;

        // TODO fall through

    }

    public override void Abort() { }

    #endregion
}