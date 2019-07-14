using UnityEngine;

public class PlayerGrounded : ControllerState {

    public int m_dashAmount = 0;

    #region [Consts]

    [SerializeField]
    private FloatSO SPEED;

    private const int DASH_COOL = 100;

    #endregion

    #region [PrivateVariables]

    [SerializeField] private int m_dashCooldown;

    #endregion

    // #if UNITY_EDITOR

    //     public void OnDrawGizmos() {
    //         f_ground?.OnDrawGizmos();
    //     }

    // #endif

    #region [Override]

    public override void EffectualEnter() {
        f_controller.Backwards = false;
        if (f_controller.ActiveState && f_controller.ActiveState is PlayerGrounded) {
            f_controller.Animator.Play("Walk");
        } else {
            f_controller.Animator.Play("Landing");
        }
    }

    public override void LogicalEnter() {
        f_controller.Grounded = true;
        f_controller.Velocity = Vector2.zero;
        m_dashCooldown = 0;
    }

    public override bool EnterOnCondition() {

        if (f_controller.Velocity.y > 0) {
            return false;
        }

        // The following checks if the player touches the ground
        return f_controller.GroundMovement.TryStickToGround(true);
    }

    public override bool HandleFixedUpdate() {
        if (m_dashCooldown > 0) {
            --m_dashCooldown;
        }

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

            if (m_dashCooldown == 0 && InputManager.Instance.GetButtonDown("Dash", InputManager.EDelayType.Always)) {
                m_dashCooldown = DASH_COOL;
                ++m_dashAmount;
                move *= 25;

                if (move == 0) {
                    move = 25 * (int)Mathf.Sign(f_controller.Mirror.localScale.x);
                }
            }

            if (move != 0) {
                f_controller.GroundMovement.Move(move * SPEED);
            }

            //TODO; stick to ground with for loop

        }

        return true;

        // TODO fall through

    }

    public override void Abort() { }

    #endregion
}