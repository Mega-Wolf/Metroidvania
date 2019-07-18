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

    #endregion

    #region [PrivateVariables]

    private int m_energy;
    private int m_currentHittedDuration = -1;

    #endregion

    #region [Properties]

    public int Energy {
        get {
            return m_energy;
        }
        set {
            if (SceneLoader.Instance != null) {
                return;
            }
            m_energy = Mathf.Min(value, f_maxEnergy);
            f_energyBar.Set(m_energy);
        }
    }

    public PlayerHittingDown PlayerHittingDown { get { return f_hittingDown; } }
    public PlayerHittingSide PlayerHittingSide { get { return f_hittingSide; } }
    public PlayerHittingUp PlayerHittingUp { get { return f_hittingUp; } }
    public PlayerAir PlayerAir { get { return f_air; } }
    public PlayerGrounded PlayerGrounded { get { return f_grounded; } }

    public int CurrentHitted { get { return m_currentHittedDuration; } }

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


        if (SceneLoader.Instance == null) {
            f_grounded.AddTransitionGoal("Healing", f_healing, true);
        } else {
            f_energyBar.Visibility = false;
        }

        SetStartState(f_grounded);
    }

    private void Start() {
        f_health.Add(this);
        f_health.Init(Consts.Instance.PlayerSO.HEALTH, 0, this);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (m_currentHittedDuration > 0) {
            ++m_currentHittedDuration;
            if (m_currentHittedDuration == 50) {
                m_currentHittedDuration = -1;
                SpriteRenderer.material.SetFloat("_UseReplacement", 0);
            }
        }
    }

    #endregion

    #region [Override]

    public void HandleDamage(int amount, int healthAfter, int maxHealth, Vector2 hitNormal) {
        //TODO; that looks awful

        if (Grounded) {
            ReactOnImpact(-hitNormal);
        }

        m_currentHittedDuration = 1;
        if (f_hitted.ParticleSystem) {
            f_hitted.ParticleSystem.Play();
        }
        SpriteRenderer.material.SetFloat("_UseReplacement", 1);


        //m_activeStackedState = f_hitted;
        //m_activeStackedState.LogicalEnter();
        //m_activeStackedState.EffectualEnter();
    }

    #endregion

}