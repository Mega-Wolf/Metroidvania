using UnityEngine;

public class Owl : GenericEnemy {

    #region [FinalVariables]

    [SerializeField, Autohook] private GoombaFly f_goomba;

    #endregion

    #region [Init]

    protected override void Start() {
        base.Start();

        SetStartState(f_goomba);
    }

    #endregion

}