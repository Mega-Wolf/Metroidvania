using UnityEngine;

public class PlayerHittingSide : ControllerState, IDamager {

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

    #endregion

    private void OnValidate() {
        f_damage.Init(EDamageReceiver.Enemy, (int) (FPS * (f_attackLastFrame - f_attackStartFrame)), null);
    }

    #region [Override]

    public override bool EnterOnCondition() {
        return InputManager.Instance.GetButtonDown("Fight", InputManager.EDelayType.Always);
    }

    public override void EffectualEnter() {
        f_controller.Animator.PlayInFixedTime("CharHitSide");
    }

    public override bool HandleFixedUpdate() {
        if (m_frame >= f_attackStartFrame * FPS && !m_hit) {
            m_hit = true;
            f_damage.ExecuteHit(f_playerSO.SIDE_DAMAGE, f_controller.transform.right);
        }

        ++m_frame;
        if (m_frame >= f_fullFrames * FPS) {
            return false;
        }
        return true;
    }

    public override void LogicalEnter() {
        m_frame = 0;
        m_hit = false;
    }

    public void Damaged(Health health) {
        
    }

    #endregion

}