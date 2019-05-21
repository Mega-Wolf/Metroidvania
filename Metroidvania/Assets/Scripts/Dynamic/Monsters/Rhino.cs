using UnityEngine;

public class Rhino : GenericEnemy {

    #region [Consts]

    [SerializeField]
    private RhinoSO f_rhinoSO;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private GoombaWalk f_goomba;

    [SerializeField, Autohook]
    private Charge f_charge;

    #endregion

    #region [Init]

    protected override void Start() {
        base.Start();

        f_goomba.AddTransitionGoal("Charge", f_charge);
        f_charge.AddTransitionGoal("Goomba", f_goomba);

        SetStartState(f_goomba);
    }

    #endregion
}