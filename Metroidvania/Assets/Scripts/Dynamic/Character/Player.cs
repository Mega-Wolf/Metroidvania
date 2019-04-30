using UnityEngine;

public class Player : Controller {

    #region [Init]

    private void Awake() {

        PlayerAir air = new PlayerAir(this);
        PlayerGrounded grounded = new PlayerGrounded(this);

        air.AddTransitionGoal("Grounded", grounded);

        grounded.AddTransitionGoal("Air", air);

        SetStartState(grounded);
        
    }

    #endregion

}