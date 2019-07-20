using UnityEngine;

public class PlayerHittingUp : ControllerState, IDamager {

    public int m_hitAmount = 0;
    public int m_hittedAmount = 0;

    #region [Consts]

    [SerializeField]
    private PlayerSO f_playerSO;

    #endregion

    #region [MemberFields]

    [SerializeField]
    private int f_fps;
    private float FPS { get { return 5 / 6f * f_fps; } }

    [SerializeField]
    private int f_fullFrames;

    [SerializeField]
    private int f_attackStartFrame;

    [SerializeField]
    private int f_attackLastFrame;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private Damage f_damage;

    #endregion

    #region [PrivateVariables]

    private int m_frame = 0;
    private bool m_hit;

    private bool m_damaged = false;

    #endregion

    #region [Init]

    private void Start() {
        OnValidate();
    }

    private void OnValidate() {
        f_damage.Init(EDamageReceiver.Enemy, (int)(FPS * (f_attackLastFrame - f_attackStartFrame)), this);
    }

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {

        bool isTrue = false;

        if (SceneLoader.Instance && SceneLoader.Instance.BossFightValue == SceneLoader.BossFight.Owl) {
            isTrue = InputManager.Instance.GetButtonDown("Fight", InputManager.EDelayType.Always);
        } else {
            isTrue = InputManager.Instance.GetButton("Up") && InputManager.Instance.GetButtonDown("Fight", InputManager.EDelayType.Always);
        }

        if (isTrue) {
            ++m_hitAmount;
            return true;
        }
        return false;
    }

    public override void EffectualEnter() {
        f_controller.Backwards = false;
        f_controller.Animator.PlayInFixedTime("CharHitUp");
    }

    public override bool HandleFixedUpdate() {
        if (m_frame >= f_attackStartFrame * FPS && !m_hit) {
            m_hit = true;
            f_damage.ExecuteHit(f_playerSO.UP_DAMAGE, Vector2.up);
        }

        ++m_frame;
        if (m_frame >= f_fullFrames * FPS) {
            return false;
        }
        return true;
    }

    public override void LogicalEnter() {
        OnValidate();
        m_frame = 0;
        m_hit = false;
        m_damaged = false;
    }

    public override void Abort() { }

    public void Damaged(IDamageTaker health) {
        if (!m_damaged) {
            ++m_hittedAmount;
            m_damaged = true;
        }
    }

    #endregion

}