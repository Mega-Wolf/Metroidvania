using UnityEngine;

public class PlayerGrounded : ControllerState {

    #region [Consts]

    [SerializeField]
    private PlayerSO f_playerSO;

    #endregion

    #region [FinalVariables]

    private GroundMovementRaycast f_ground;

    #endregion

    #region [Init]

    private void Awake() {
        f_ground = new GroundMovementRaycast(f_controller, f_controller.Height, f_controller.HalfWidth);
    }
    
    #endregion

// #if UNITY_EDITOR

//     public void OnDrawGizmos() {
//         f_ground.OnDrawGizmos();
//     }

// #endif

    #region [Override]

    public override void EffectualEnter() {
        f_controller.Animator.Play("Landing");
    }

    public override void LogicalEnter() {
        f_controller.Grounded = true;
        f_controller.Backwards = false;
        f_controller.Velocity = Vector2.zero;
    }

    public override bool EnterOnCondition() {

        if (f_controller.Velocity.y >= 0) {
            return false;
        }

        // The following checks if the player touches the ground
        return f_ground.TryStickToGround();
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
                f_ground.Move(move == 1);
            }

        }

        return true;

        // TODO Heal

        // TODO Hit sideways up

        // TODO fall through

    }

    #endregion
}