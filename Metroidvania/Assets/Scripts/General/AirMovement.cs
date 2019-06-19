using UnityEngine;

public class AirMovement : Movement {

    #region [FinalVariables]

    private Controller f_controller;

    #endregion

    #region [PrivateVariables]

    private bool m_smooth = true;

    #endregion

    #region [Properties]

    public Vector2 Goal { get; set; }

    public override LayerMask Mask { get { return LayerMask.GetMask("Default" /*, "MonsterTransparent" */); } }

    #endregion

    #region [Constructors]

    public AirMovement(Controller controller) {
        f_controller = controller;
    }

    #endregion

#if UNITY_EDITOR
    public override void OnDrawGizmos() {
        Debug.DrawLine(f_controller.transform.position, Goal, Color.green);
    }
#endif

    #region [Override]

    public override void AirMove() { }

    #endregion

    #region [PublicMethods]

    public void SetSmooth(bool smooth) {
        m_smooth = smooth;
    }

    public void Move(float speed) {
        //f_controller.Velocity = (Goal - (Vector2)f_controller.transform.position).normalized * speed;

        if (m_smooth) {
            Vector2 vel = f_controller.Velocity;
            Vector2 goal = Vector2.SmoothDamp(f_controller.transform.position, Goal, ref vel, 0.25f, speed);
            f_controller.Velocity = (goal - (Vector2)f_controller.transform.position).normalized * speed;
        } else {
            f_controller.Velocity = (Goal - (Vector2)f_controller.transform.position).normalized * speed;
        }


    }

    #endregion

}