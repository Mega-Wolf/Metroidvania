using UnityEngine;

public class AirMovement : Movement {

    #region [FinalVariables]

    private Controller f_controller;

    #endregion

    #region [Properties]

    public Vector2 Goal { get; set; }

    public override LayerMask Mask { get { return LayerMask.GetMask("Default", "MonsterTransparent"); } }

    #endregion

    #region [Constructors]

    public AirMovement(Controller controller) {
        f_controller = controller;
    }

    #endregion

    #region [Override]

    public override void AirMove() { }

    #endregion

    #region [PublicMethods]

    public void Move(Vector2 speed) {
        f_controller.Velocity = (Goal - (Vector2)f_controller.transform.position).normalized * speed;
    }

    #endregion

}