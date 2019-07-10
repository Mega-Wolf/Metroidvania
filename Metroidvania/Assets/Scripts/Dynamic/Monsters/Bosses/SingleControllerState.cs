using UnityEngine;

public class SingleControllerState : GenericEnemy {

    #region [MemberFields]

    [SerializeField] private ControllerState f_singleState;

    #endregion

    #region [FinalVariables]


    [SerializeField, Autohook] CharacterHitted f_hitted;

    #endregion

    #region [Override]

    public override void Start() {
        base.Start();

        SetStartState(f_singleState);
    }

    public override void HandleDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        if (healthAfter <= 0) {
            Debug.LogWarning(gameObject.name + " died in frame: " + GameManager.Instance.Frame + "; Char had " + Consts.Instance.Player.Health.Value + " health.");
        }
        base.HandleDamage(amount, healthAfter, maxHealth, hitNormal);

        if (f_hitted) {
            m_activeStackedState = f_hitted;
            m_activeStackedState.LogicalEnter();
            m_activeStackedState.EffectualEnter();
        }

    }

    #endregion

}