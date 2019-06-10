using UnityEngine;

public class FeatherFight : ControllerState {

    #region [MemberFields]

    [SerializeField] private BoxCollider2D f_rect;

    [SerializeField] private float f_speed;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        return true;
    }

    public override void LogicalEnter() {
        SetNewGoal();
    }

    public override void EffectualEnter() {
        f_controller.Animator.Play("Fly");
    }

    public override bool HandleFixedUpdate() {

        if (((Vector2)f_controller.transform.position - f_controller.AirMovement.Goal).sqrMagnitude < 0.35f * 0.35f) {
            SetNewGoal();
        }

        f_controller.AirMovement.Move(f_speed);

        //if dist to goal < TRESHOLD
        //  set new goal

        // if timer == 0
        //      play shoot animation
        //      after x frames
        //          shoot
        //          play normal animation again

        // health < y
        //      spawn new feathers
        

        return true;
    }

    public override void Abort() {
        //won't happen
    }

    #endregion

    #region [PrivateMethods]

    private void SetNewGoal() {
        f_controller.AirMovement.Goal = new Vector2(Random.Range(f_rect.bounds.min.x, f_rect.bounds.max.x), Random.Range(f_rect.bounds.min.y, f_rect.bounds.max.y));
    }

    #endregion

}