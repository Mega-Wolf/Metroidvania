using UnityEngine;

public class CharacterHitted : ControllerState {

    private const int STUNNED_LENGTH = 0;//20;

    #region [MemberFields]

    [SerializeField] private int f_hittedLength = 50;
    [SerializeField] private Color f_hittedColor = Color.white / 2f;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook] private ParticleSystem f_particleSystem;

    private MaterialPropertyBlock f_matProp;

    #endregion

    #region [PrivateVariables]

    private int m_currentHittedDuration;
    private Vector2 m_enterVelocity;

    #endregion

    #region [Properties]

    public override bool ConsumesInputAndEffects { get { return m_currentHittedDuration < STUNNED_LENGTH; } }

    #endregion

    #region [Init]

    private void Awake() {
        f_matProp = new MaterialPropertyBlock();
    }

    #endregion

    #region [Override]

    public override void EffectualEnter() {
        //f_controller.Animator.Play("Hitted");

        //f_controller.SpriteRenderer.color = f_hittedColor;
        f_controller.SpriteRenderer.material.SetFloat("_UseReplacement", 1);


//	if (f_particleSystem) {
//	        f_controller.SpriteRenderer.material.SetFloat("_ColorMode", 4);
//            f_controller.SpriteRenderer.GetPropertyBlock(f_matProp);
//            f_matProp.SetFloat("_ColorMode", 4);
//            f_controller.SpriteRenderer.SetPropertyBlock(f_matProp);
//	}
    }

    public override void LogicalEnter() {
        m_currentHittedDuration = 0;
        if (f_particleSystem) {
            f_particleSystem.Play();
        }
        //m_enterVelocity = f_controller.Velocity;
        //f_controller.Velocity = Vector2.zero;
    }

    public override bool EnterOnCondition() {
        // This is a bit weird, since that is started from outside
        return true;
    }

    public override bool HandleFixedUpdate() {
        if (m_currentHittedDuration == f_hittedLength) {
            return false;
        }
        //f_controller.Velocity = m_enterVelocity;
        ++m_currentHittedDuration;
        return true;
    }

    public override void Abort() {
        //f_controller.SpriteRenderer.color = Color.white;
        f_controller.SpriteRenderer.material.SetFloat("_UseReplacement", 0);


//	if (f_particleSystem) {
//	        f_controller.SpriteRenderer.material.SetFloat("_ColorMode", 0);
//            f_controller.SpriteRenderer.GetPropertyBlock(f_matProp);
//            f_matProp.SetFloat("_ColorMode", 0);
//            f_controller.SpriteRenderer.SetPropertyBlock(f_matProp);
//	}
    }

    #endregion
}