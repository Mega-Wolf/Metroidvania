using UnityEngine;

public class PlayerAir : ControllerState {

    #region [Consts]

    private const float EXTRA_RAY_LENGTH = 0.1f;

    private const float MAX_SPEED = 1.5f;
    

    private const float JUMP_HEIGHT = 2.5f;
    private const float JUMP_HALF_DURATION = 0.45f;

    private const float JUMP_SPEED = 2 * JUMP_HEIGHT / JUMP_HALF_DURATION;

    //private const float MAX_FALL_SPEED = JUMP_SPEED* 1.5f;
    private const float MAX_FALL_SPEED = 5;

    private Vector2 ACCELERATION = new Vector2(10f, JUMP_SPEED / JUMP_HALF_DURATION);
    
    

    // this is intended to define the speed of falling down; however, everything else turn in an instant as well
    //private float TURN_FACTOR

    #endregion

    #region [PrivateVariables]

    private bool m_isJumping;

    #endregion

    #region [Constructors]

    public PlayerAir(Controller controller) : base(controller) {
    }

    #endregion

    #region [Override]

    public override void Enter() {
        f_controller.transform.rotation = Quaternion.identity;

        if (!m_isJumping) {
            // this means that the jump animation is already playing
            f_controller.Animator.Play("Air");
        }

    }

    protected override bool EnterOnCondition() {

        if (InputManager.Instance.GetButtonDown("Jump")) {
            m_isJumping = true;
            f_controller.Animator.Play("Jump");
            f_controller.Velocity = new Vector2(0, JUMP_SPEED);
            Enter();
            return true;
        }

        // The following checks if the player touches the ground
        {
            Vector2 origin = f_controller.transform.TransformPoint(new Vector2(0, (f_controller.Height / 2f - EXTRA_RAY_LENGTH) / 2f));

            LayerMask GROUND_MASK = LayerMask.GetMask("Default");

            RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(f_controller.HalfWidth * 2f, f_controller.Height / 2f + EXTRA_RAY_LENGTH), 0, Vector2.zero, f_controller.transform.eulerAngles.z, GROUND_MASK);

            //TODO Enter()

            return !hit;
        }
    }

    public override void HandleFixedUpdate() {

        Vector2 velocity = f_controller.Velocity;

        // Horizontal Movement
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

            // maybe really just set the rigidbody velocity?

            velocity.x += move * 1 / 60f * ACCELERATION.x;

            velocity.x = Mathf.Clamp(velocity.x, -MAX_SPEED, MAX_SPEED);
        }

        // vertical movement
        {
            //  TODO end jump at specific time (dynamic)

            //y velocity according to timeline?
            velocity.y = velocity.y - ACCELERATION.y / 60f;

            velocity.y = Mathf.Max(velocity.y, -MAX_FALL_SPEED);
        }

        f_controller.Velocity = velocity;

        // hit downwards
        // hit upwards
        // hit left
        // hit right

    }


    // public override void Leave() { }

    #endregion

}