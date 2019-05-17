using UnityEngine;

// why is that its own class
// it could also just use the controller; because that wouln't change anything about it?
// Will think about that

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