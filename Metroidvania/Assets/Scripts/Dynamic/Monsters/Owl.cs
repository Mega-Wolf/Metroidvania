using UnityEngine;

public class Owl : GenericEnemy {

    #region [FinalVariables]

    [SerializeField, Autohook] private GoombaFly f_goomba;

    #endregion

    #region [Init]

    public override void Start() {
        base.Start();

        SetStartState(f_goomba);
    }

    #endregion

}