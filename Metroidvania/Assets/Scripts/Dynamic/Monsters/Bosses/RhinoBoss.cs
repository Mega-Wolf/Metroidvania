using UnityEngine;

public class RhinoBoss : GenericEnemy {

    #region [Consts]

    [SerializeField]
    private RhinoSO f_rhinoSO;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private bool f_startRoll = false;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private GoombaWalk f_goomba;

    [SerializeField, Autohook]
    private Charge f_charge;

    [SerializeField, Autohook]
    private Roll f_roll;

    [SerializeField, Autohook]
    private CharacterHitted f_hitted;

    #endregion

    #region [PrivateVariables]

    private bool m_isRolling;

    #endregion

    #region [Init]

    public override void Start() {
        base.Start();

        f_goomba.AddTransitionGoal("Charge", f_charge);
        f_charge.AddTransitionGoal("Goomba", f_goomba);

        if (f_startRoll) {
            SetStartState(f_roll);
            m_isRolling = true;
        } else {
            SetStartState(f_goomba);
            m_isRolling = false;
        }

    }

    #endregion

    #region [Override]

    public override void HandleDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {

        base.HandleDamage(amount, healthAfter, maxHealth, hitNormal);

        if (m_isRolling) {
            SwitchState(false);
        } else {
            if (m_activeState == f_goomba) {
                SwitchState(f_charge);
            }
        }

        m_activeStackedState = f_hitted;
        m_activeStackedState.LogicalEnter();
        m_activeStackedState.EffectualEnter();
    }

    #endregion

    #region [PublicMethods]

    public void SwitchState(bool rolling) {
        if (rolling && !m_isRolling) {
            m_isRolling = true;
            SwitchState(f_roll);
            return;
        }


        //TOOD; maybe charge
        if (!rolling && m_isRolling) {
            m_isRolling = false;
            SwitchState(f_goomba);
            return;
        }
    }

    #endregion

    private void SwitchState(ControllerState newState) {
        m_activeState.Abort();
        m_activeState = newState;
        m_activeState.LogicalEnter();
        m_activeState.EffectualEnter();
    }


}