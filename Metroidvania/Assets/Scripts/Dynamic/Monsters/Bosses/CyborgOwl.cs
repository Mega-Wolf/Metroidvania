using UnityEngine;

public class CyborgOwl : GenericEnemy {

    #region [FinalVariables]

    [SerializeField, Autohook] private FeatherFight f_featherFight;

    #endregion

    #region [Override]

    protected override void Start() {
        base.Start();

        SetStartState(f_featherFight);
    }

    #endregion

}