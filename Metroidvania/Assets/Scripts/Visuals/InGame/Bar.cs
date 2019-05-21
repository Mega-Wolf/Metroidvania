using NaughtyAttributes;
using UnityEngine;

public partial class Consts {
    public BarSO HealthBar;
}

public class Bar : MonoBehaviour {

    #region [Consts]

    [SerializeField]
    private BarSO f_barSO;

    private float TIME_VALUE_CHANGE { get { return f_barSO.TimeValueChange; } }

    #endregion

    #region [MemberFields]

    [SerializeField, Autohook]
    private SpriteRenderer f_renderer;

    #endregion

    #region [FinalVariables]

    MaterialPropertyBlock f_matProp;

    private int f_maxValue;

    #endregion

    #region [PrivateVariables]

    private int m_value;
    private int m_showValue;
    private float m_changeTime;

    #endregion

    #region [Init]

    private void Awake() {
        f_matProp = new MaterialPropertyBlock();
    }

    public void Init(int maxValue) {
        f_maxValue = maxValue;
        m_value = maxValue;
        m_showValue = maxValue;
        f_renderer.enabled = false;

        UpdateShaderProperties();
        enabled = false;
    }

    #endregion

    #region [Updates]

    private void Update() {
        if (m_changeTime + TIME_VALUE_CHANGE <= Time.time) {
            m_showValue = m_value;
            UpdateShaderProperties();
            enabled = false;
        }
    }

    #endregion

    #region [PrivateMethods]

    private void UpdateShaderProperties() {
        
        if (m_value == f_maxValue && m_showValue == f_maxValue)  {
            f_renderer.enabled = false;
            return;
        }

        f_renderer.GetPropertyBlock(f_matProp);
        f_matProp.SetFloat("_Real", m_value / (float)f_maxValue);
        f_matProp.SetFloat("_Show", m_showValue / (float)f_maxValue);
        f_renderer.SetPropertyBlock(f_matProp);
    }

    #endregion

    #region [PublicMethods]

    public void Set(int value) {
        f_renderer.enabled = true;

        m_showValue = m_value;
        m_value = value;

        m_changeTime = Time.time;
        UpdateShaderProperties();
        enabled = true;
    }

    #endregion

}