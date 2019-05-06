using UnityEngine;

public class PlayerGrounded : ControllerState {

    #region [Consts]

    private const float EXTENDS = 0.05f * 2f;
    private const float EXTRA_RAY_LENGTH = 0.1f;

    private const float SPEED = 3f;

    #endregion

    #region [FinalVariables]

    private GroundMovementRaycast f_ground;

    #endregion

    #region [Constructors]

    public PlayerGrounded(Controller controller) : base(controller) {
        f_ground = new GroundMovementRaycast(controller, controller.Height, controller.HalfWidth, SPEED);
    }

    #endregion

#if UNITY_EDITOR

    public override void OnDrawGizmos() {
        f_ground.OnDrawGizmos();
    }

#endif

    #region [Override]

    public override void Enter() {
        f_controller.Backwards = false;
        f_controller.Velocity = Vector2.zero;

        //TODO; don't
        f_controller.Animator.Play("Landing");
    }

    protected override bool EnterOnCondition() {

        if (f_controller.Velocity.y >= 0) {
            return false;
        }

        // The following checks if the player touches the ground
        {
            bool hit = f_ground.TryStickToGround();

            if (hit) {
                Enter();
                return true;
            }

            return false;
        }
    }

    public override void HandleFixedUpdate() {

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
                f_ground.Move(move == 1);
            }

        }

        return;

        // TODO Heal

        // TODO Hit sideways up

        // TODO fall through

    }

    #endregion
}