using System.Collections.Generic;
using UnityEngine;

// This only gets faked at the moment
public class Roll : ControllerState {

    #region [MemberFields]

    [SerializeField]
    private Collider2D f_rollCollider;

    [SerializeField]
    private Collider2D f_normalCollider;

    [SerializeField]
    private int m_rotFrames;

    [SerializeField]
    private float m_rollSpeed = 10;

    [SerializeField]
    private bool m_rollingRight;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private SpriteRenderer f_spriteRenderer;

    #endregion

    #region [PrivateVariables]

    private int m_currentRotFrames;

    #endregion

    #region [Override]

    public override bool EnterOnCondition() {
        return true;
    }

    public override void LogicalEnter() {
        m_currentRotFrames = 0;
    }

    public override void EffectualEnter() {
        f_spriteRenderer.enabled = true;
        f_controller.SpriteRenderer.enabled = false;
        f_rollCollider.enabled = true;
        f_normalCollider.enabled = false;
    }

    public override bool HandleFixedUpdate() {
        //TODO; this is such a hack
        if (InputManager.Instance.IgnoreInput) {
            return true;
        }

        m_currentRotFrames += m_rollingRight ? -1 : 1;
        transform.localEulerAngles = new Vector3(0, 0, (float)m_currentRotFrames / m_rotFrames * 360);

        if (!f_controller.Grounded) {
            return true;
        }

        if (Mathf.Abs(f_controller.Velocity.x) < 0.1f) {
            m_rollingRight = !m_rollingRight;
        }

        //TODO; does not care about transform
        if (f_controller.Velocity.x < -0.1f) {
            m_rollingRight = false;
        } else if (f_controller.Velocity.x > 0.1f) {
            m_rollingRight = true;
        }

        f_controller.Velocity = Vector2.zero;
        f_controller.GroundMovement.Move((m_rollingRight ? 1 : -1) * m_rollSpeed);

        return true;
    }

    public override void Abort() {
        f_controller.Velocity = Vector2.zero;
        f_spriteRenderer.enabled = false;
        f_controller.SpriteRenderer.enabled = true;
        f_rollCollider.enabled = false;
        f_normalCollider.enabled = true;
    }

    #endregion



}