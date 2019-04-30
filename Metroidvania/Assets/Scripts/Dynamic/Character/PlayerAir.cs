using UnityEngine;

public class PlayerAir : ControllerState {

    #region [Consts]

    private const float EXTENDS = 0.05f * 2f;
    private const float EXTRA_RAY_LENGTH = 0.1f;

    private Vector2 ACCELERATION = new Vector2(5, 4);
    private Vector2 MAX_VELOCITY = new Vector2(5, 4);

    // this is intended to define the speed of falling down; however, everything else turn in an instant as well
    //private float TURN_FACTOR

    #endregion

    #region [PrivateVariables]

    private bool m_endedJump;

    #endregion

    #region [Constructors]

    public PlayerAir(Controller controller) : base(controller) {
    }

    #endregion

    #region [Override]

    public override void Enter() {
        f_controller.transform.rotation = Quaternion.identity;
        //f_controller.Animator.Play("JumpStart");
    }

    protected override bool EnterOnCondition() {

        if (InputManager.Instance.GetButtonDown("Jump")) {
            Enter();
            return true;
        }

        // The following checks if the player touches the ground
        {
            Vector2 origin = f_controller.transform.TransformPoint(new Vector2(0, (f_controller.Height / 2f - EXTRA_RAY_LENGTH) / 2f));

            LayerMask GROUND_MASK = LayerMask.GetMask("Default");

            RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(f_controller.HalfWidth * 2f - EXTENDS, f_controller.Height / 2f + EXTRA_RAY_LENGTH), 0, Vector2.zero, f_controller.transform.eulerAngles.z, GROUND_MASK);

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

            //TODO
        }

        // hit downwards
        // hit upwards
        // hit left
        // hit right

    }


    // public override void Leave() { }

    #endregion

}