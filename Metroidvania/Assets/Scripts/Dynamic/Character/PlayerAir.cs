// Not caring about slopes; changing horizontal velocity all the time
#define ORI_JUMP

#if !ORI_JUMP

// slope will define the initial jump velocity; can only change horizontal velocity when falling
#define SONIC_JUMP
#endif

using UnityEngine;

public class PlayerAir : ControllerState {

    #region [Consts]

    private const int COYOTE_FRAMES = 5;

    private const float EXTRA_RAY_LENGTH = 0.1f;

    private const float MAX_SPEED = 3f;


    private const float JUMP_HEIGHT = 2.5f;
    private const float JUMP_HALF_DURATION = 0.45f;

    private const float JUMP_SPEED = 2 * JUMP_HEIGHT / JUMP_HALF_DURATION;

    private const float MAX_FALL_SPEED = JUMP_SPEED * 1.5f;
    //private const float MAX_FALL_SPEED = 5;

    //TODO; ACCELERATION.x is not used
    private Vector2 ACCELERATION = new Vector2(5f, JUMP_SPEED / JUMP_HALF_DURATION);

    #endregion

    #region [PrivateVariables]

    private bool m_playJumpAnimation;
    private bool m_coyoteable;

    #endregion

    #region [Constructors]

    public PlayerAir(Controller controller) : base(controller) {
    }

    #endregion

    #region [Override]

    public override void EffectualEnter() {
        if (m_playJumpAnimation) {
            m_playJumpAnimation = false;
            f_controller.Animator.Play("Jump");
        } else {
            f_controller.Animator.Play("Air");
        }
    }

    public override void LogicalEnter() {
        f_controller.Grounded = false;
        f_controller.Backwards = false;
        f_controller.transform.rotation = Quaternion.identity;
    }

    public override bool EnterOnCondition() {

        if (InputManager.Instance.GetButtonDown("Jump", InputManager.EDelayType.OnlyWhenDown)) {
            m_playJumpAnimation = true;
            m_coyoteable = false;

#if ORI_JUMP
            f_controller.Velocity = new Vector2(f_controller.Velocity.x, JUMP_SPEED);
#elif SONIC_JUMP
            f_controller.Velocity = f_controller.Velocity + JUMP_SPEED * (Vector2)f_controller.transform.up;
#endif

            return true;
        }

        // The following checks if the player touches the ground
        {
            Vector2 origin = f_controller.transform.TransformPoint(new Vector2(0, (f_controller.Height / 2f - EXTRA_RAY_LENGTH) / 2f));

            LayerMask GROUND_MASK = LayerMask.GetMask("Default");

            // angles is actually always 0
            RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(f_controller.HalfWidth * 2f, f_controller.Height / 2f + EXTRA_RAY_LENGTH), f_controller.transform.eulerAngles.z, Vector2.zero, 0, GROUND_MASK);

            if (!hit) {
                m_playJumpAnimation = false;

                // Only delayable if not started by a jump
                m_coyoteable = true;

                return true;
            }

            return false;
        }
    }

    public override bool HandleFixedUpdate() {

        // Late jump (coyote time)
        if (m_coyoteable && (f_controller.StateStartedFrame + COYOTE_FRAMES >= GameManager.Instance.Frame) && InputManager.Instance.GetButtonDown("Jump", InputManager.EDelayType.OnlyWhenDown)) {
            m_coyoteable = false;
            m_playJumpAnimation = true;

            // since in the air, the upwards transform will always be directly up, ther is no difference between Ori and Sonic
            f_controller.Velocity = new Vector2(f_controller.Velocity.x, JUMP_SPEED);
        }

        Vector2 velocity = f_controller.Velocity;

        // Horizontal Movement
#if SONIC_JUMP
            if (velocity.y < 0)
            //if (velocity.y < JUMP_SPEED / 2f) {
#endif
        {
            int move = 0;
            if (InputManager.Instance.GetButton("Left")) {
                move = -1;
            }
            if (InputManager.Instance.GetButton("Right")) {
                ++move;
            }

            // this movement will not set the velocity directly, but will change the acceleration of the character
            // from where do I get the acceleration/velocity
            // oh well this state should just have the acceleration itself
            // but the velocity should come from outside; otherwise I would rapidly slow down when jumping
            // TODO; change that comment above, since it seems that I don't do it now

            //velocity.x += move * 1 / 60f * ACCELERATION.x;
            velocity.x = move * MAX_SPEED;

            //velocity.x = Mathf.Clamp(velocity.x, -MAX_SPEED, MAX_SPEED);
        }

        // vertical movement
        {
            velocity.y = velocity.y - ACCELERATION.y / 60f;

            if (velocity.y > 0 && InputManager.Instance.GetButtonUp("Jump")) {
                //velocity.y = -velocity.y;
                //velocity.y = 0;
                velocity.y = Mathf.Min(velocity.y, 0.5f);
            }

            velocity.y = Mathf.Max(velocity.y, -MAX_FALL_SPEED);
        }

        f_controller.Velocity = velocity;

        // hit downwards
        // hit upwards
        // hit left
        // hit right

        return true;

    }


    // public override void Leave() { }

    #endregion

}