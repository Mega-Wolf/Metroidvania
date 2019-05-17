using NaughtyAttributes;
using UnityEngine;

public partial class Consts {
    public HealthBarSO HealthBar;
}

public class HealthBar : MonoBehaviour {

    #region [Consts]

    [SerializeField]
    private HealthBarSO f_healthBarSO;

    private float TIME_HEALTH_CHANGE { get { return f_healthBarSO.TimeHealthChange; } }

    #endregion

    #region [MemberFields]

    [SerializeField, Autohook]
    private SpriteRenderer f_renderer;

    #endregion

    #region [FinalVariables]

    MaterialPropertyBlock f_matProp;

    private int f_maxHealth;

    #endregion

    #region [PrivateVariables]

    private int m_health;
    private int m_showHealth;
    private float m_changeTime;

    #endregion

    #region [Init]

    private void Awake() {
        f_matProp = new MaterialPropertyBlock();
    }

    public void Init(int maxHealth) {
        f_maxHealth = maxHealth;
        m_health = maxHealth;
        m_showHealth = maxHealth;
        f_renderer.enabled = false;

        UpdateShaderProperties();
        enabled = false;
    }

    #endregion

    #region [Updates]

    private void Update() {
        if (m_changeTime + TIME_HEALTH_CHANGE <= Time.time) {
            m_showHealth = m_health;
            UpdateShaderProperties();
            enabled = false;
        }
    }

    #endregion

    #region [PrivateMethods]

    private void UpdateShaderProperties() {
        
        if (m_health == f_maxHealth && m_showHealth == f_maxHealth)  {
            f_renderer.enabled = false;
            return;
        }

        f_renderer.GetPropertyBlock(f_matProp);
        f_matProp.SetFloat("_Real", m_health / (float)f_maxHealth);
        f_matProp.SetFloat("_Show", m_showHealth / (float)f_maxHealth);
        f_renderer.SetPropertyBlock(f_matProp);
    }

    #endregion

    #region [PublicMethods]

    public void SetHealth(int health) {
        f_renderer.enabled = true;

        m_showHealth = m_health;
        m_health = health;

        m_changeTime = Time.time;
        UpdateShaderProperties();
        enabled = true;
    }

    #endregion

}