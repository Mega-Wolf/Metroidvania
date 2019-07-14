using UnityEngine;

public class PlayerHittingDown : ControllerState, IDamager {

    public int m_hitAmount = 0;
    public int m_hittedAmount = 0;

    #region [MemberFields]

    [SerializeField]
    private int f_fps;

    private float FPS { get { return 5 / 6f * f_fps; } }

    [SerializeField]
    private int f_startFrame = 1;

    [SerializeField]
    private int f_lastFrame = 3;

    [SerializeField]
    private int f_maxLength = 5;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Damage f_damage;

    #endregion

    #region [PrivateVariables]

    private int m_frame = 0;
    private bool m_damaged;
    private bool m_hit;

    #endregion

    #region [Init]

    private void Start() {
        OnValidate();
    }

    private void OnValidate() {
        f_damage.Init(EDamageReceiver.Enemy, (int)(FPS * (f_lastFrame - f_startFrame)), this);
    }

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        if (InputManager.Instance.GetButton("Down") && InputManager.Instance.GetButtonDown("Fight", InputManager.EDelayType.Always)) {
            ++m_hitAmount;
            return true;
        }
        return false;
    }

    public override void LogicalEnter() {
        //f_damage.Init(EDamageReceiver.Enemy, (int)(FPS * (f_lastFrame - f_startFrame)), this);
        OnValidate();

        m_frame = 0;
        m_damaged = false;
        m_hit = false;
    }

    public override void EffectualEnter() {
        f_controller.Backwards = false;
        f_controller.Animator.PlayInFixedTime("CharHitDown");
    }

    public override bool HandleFixedUpdate() {
        if (m_frame >= f_startFrame * FPS && !m_hit) {
            m_hit = true;
            f_damage.ExecuteHit(Consts.Instance.PlayerSO.DOWN_DAMAGE, Vector2.down);
        }

        ++m_frame;
        if (m_frame >= f_maxLength * FPS) {
            return false;
        }
        return true;
    }

    public void Damaged(IDamageTaker health) {
        if (!m_damaged) {
            ++m_hittedAmount;
            m_damaged = true;
            f_controller.Velocity = new Vector2(f_controller.Velocity.x, Consts.Instance.PlayerSO.PLAYER_AIR.JUMP_SPEED / 1.5f);
        }
    }

    public override void Abort() { }

    #endregion

}