using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles getting damage
/// It will turn invincible after getting hit
/// </summary>
public class InvincibleHealth : Health {

    #region [MemberFields]

    [SerializeField]
    private int f_invincibleFrames;

    #endregion

    #region [FinalVariables]

    //[SerializeField, Autohook]
    //private SpriteRenderer f_spriteRenderer;

    #endregion

    #region [PrivateVariables]

    private int m_invincibleTime = -1;
    //private int m_lastDamage;

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        if (m_invincibleTime == f_invincibleFrames) {
            enabled = false;
            //f_spriteRenderer.color = Color.white;
            m_invincibleTime = -1;
            return;
        }

        ++m_invincibleTime;
    }

    #endregion

    #region [PublicMethods]

    public override void TakeDamage(int amount, Vector2 hitNormal /* , damageDealer(not always possible (projectils), maybe damageDealer = hitEffect) */ ) {
        if (m_invincibleTime == -1) {
            base.TakeDamage(amount, hitNormal);
            enabled = true;
            //f_spriteRenderer.color = Color.white / 2f;
            m_invincibleTime = 0;
        }
    }

    #endregion

}