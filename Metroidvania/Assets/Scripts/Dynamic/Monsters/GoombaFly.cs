using UnityEngine;

/// <summary>
/// Flying around as a monster like a Goomba (but up/down)
/// </summary>
public class GoombaFly : ControllerState {

    #region [MemberFields]

    [SerializeField] private float speed;

    #endregion

    #region [PrivateVariable]

    private bool m_movingUp;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        return true;
    }

    public override void LogicalEnter() {}

    public override void EffectualEnter() {
        f_controller.Animator.Play("Fly");
    }

    public override bool HandleFixedUpdate() {

        // if (InputManager.Instance.IgnoreInput) {
        //     return true;
        // }

        if (Mathf.Abs(f_controller.Velocity.y) < 0.1f) {
            //if (f_controller.Velocity.y != 0) {
                m_movingUp = !m_movingUp;
            //}
        }

        if (m_movingUp) {
            f_controller.AirMovement.Goal = (Vector2)f_controller.transform.position + Vector2.up;
        } else {
            f_controller.AirMovement.Goal = (Vector2)f_controller.transform.position + Vector2.down;
        }

        f_controller.Velocity = Vector2.zero;

        f_controller.AirMovement.Move(speed);

        return true;
    }

    public override void Abort() {}

    #endregion

}