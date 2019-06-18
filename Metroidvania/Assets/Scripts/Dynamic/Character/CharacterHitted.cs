using UnityEngine;

public class CharacterHitted : ControllerState {

    private const int STUNNED_LENGTH = 0;//20;
    private const int HITTED_LENGTH = 50;

    #region [PrivateVariables]

    private int m_currentHittedDuration;
    private Vector2 m_enterVelocity;

    #endregion

    #region [Properties]

    public override bool ConsumesInputAndEffects { get { return m_currentHittedDuration < STUNNED_LENGTH; } }

    #endregion

    #region [Override]

    public override void EffectualEnter() {
        //f_controller.Animator.Play("Hitted");
        f_controller.SpriteRenderer.color = Color.white / 2f;
    }

    public override void LogicalEnter() {
        m_currentHittedDuration = 0;
        //m_enterVelocity = f_controller.Velocity;
        //f_controller.Velocity = Vector2.zero;
    }

    public override bool EnterOnCondition() {
        // This is a bit weird, since that is started from outside
        return true;
    }

    public override bool HandleFixedUpdate() {
        if (m_currentHittedDuration == HITTED_LENGTH) {
            return false;
        }
        //f_controller.Velocity = m_enterVelocity;
        ++m_currentHittedDuration;
        return true;
    }

    public override void Abort() {
        f_controller.SpriteRenderer.color = Color.white;
    }

    #endregion
}