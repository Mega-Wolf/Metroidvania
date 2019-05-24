using UnityEngine;

public class Player : Controller, IDamagable {

    #region [MemberFields]

    //TODO
    [SerializeField]
    private int f_maxEnergy;

    [SerializeField]
    private PlayerHittingSide f_hittingSide;

    [SerializeField]
    private PlayerHittingUp f_hittingUp;

    [SerializeField]
    private Bar f_energyBar;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private PlayerAir f_air;

    [SerializeField, Autohook]
    private PlayerGrounded f_grounded;

    [SerializeField, Autohook]
    private PlayerHittingDown f_hittingDown;

    [SerializeField, Autohook]
    private PlayerHealing f_healing;

    [SerializeField, Autohook]
    private CharacterHitted f_hitted;

    [SerializeField, Autohook]
    private Health f_health;

    #endregion

    #region [PrivateVariables]

    private int m_energy;

    #endregion

    #region [Properties]

    public int Energy {
        get {
            return m_energy;
        }
        set {
            m_energy = Mathf.Min(value, f_maxEnergy);
            f_energyBar.Set(m_energy);
        }
    }

    public Health Health { get { return f_health; } }

    #endregion

    #region [Init]

    protected override void Awake() {
        base.Awake();

        f_energyBar.Init(f_maxEnergy, 0);
        Energy = 10; //TESTING

        Consts.Instance.Player = this;

        f_air.AddTransitionGoal("Grounded", f_grounded);
        f_grounded.AddTransitionGoal("Air", f_air);

        f_air.AddTransitionGoal("CharHittingDown", f_hittingDown, true);
        f_air.AddTransitionGoal("CharHittingUp", f_hittingUp, true);
        f_air.AddTransitionGoal("CharHittingSide", f_hittingSide, true);

        f_grounded.AddTransitionGoal("CharHittingUp", f_hittingUp, true);
        f_grounded.AddTransitionGoal("CharHittingSide", f_hittingSide, true);
        f_grounded.AddTransitionGoal("Healing", f_healing, true);

        SetStartState(f_grounded);
    }

    private void Start() {
        f_health.Add(this);
        f_health.Init(Consts.Instance.PlayerSO.HEALTH, 0);
    }

    #endregion

    #region [Override]

    public void TakeDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        //TODO; that looks awful
        ReactOnImpact(-hitNormal);
        // m_activeStackedState = f_hitted;
        // m_activeStackedState.LogicalEnter();
        // m_activeStackedState.EffectualEnter();
    }

    #endregion

}