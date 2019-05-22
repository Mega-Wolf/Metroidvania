using UnityEngine;

public class PlayerHealing : ControllerState {

    private const int HEAL_FRAMES = 100;

    private const int HEAL_ENERGY_COST = 5;

    private const int HEAL_AMOUNT = 5;

    #region [MemberFields]

    [SerializeField, Autohook]
    private Bar f_bar;

    [SerializeField, Autohook]
    private SpriteRenderer f_spriteRenderer;

    #endregion

    #region [PrivateVariables]

    private int m_healedFrames;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        return ((Player)f_controller).Energy > HEAL_ENERGY_COST && InputManager.Instance.GetButtonDown("Heal", InputManager.EDelayType.Always);
    }

    public override void LogicalEnter() {
        m_healedFrames = 0;
        ((Player)f_controller).Energy -= HEAL_ENERGY_COST;
        f_bar.Init(HEAL_FRAMES, 0);
    }

    public override void EffectualEnter() { }

    public override bool HandleFixedUpdate() {
        if (m_healedFrames == HEAL_FRAMES) {
            ((Player)f_controller).Health.Heal(HEAL_AMOUNT);
            return false;
        }

        ++m_healedFrames;
        f_bar.Set(m_healedFrames);
        return true;
    }

    public override void Abort() {
        f_spriteRenderer.enabled = false;
    }

    #endregion
}