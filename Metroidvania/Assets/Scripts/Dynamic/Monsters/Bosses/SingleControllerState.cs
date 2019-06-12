using UnityEngine;

public class SingleControllerState : GenericEnemy {

    #region [FinalVariables]

    [SerializeField] private ControllerState f_singleState;

    #endregion

    #region [Override]

    protected override void Start() {
        base.Start();

        SetStartState(f_singleState);
    }

    #endregion

}