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

    [SerializeField]
    private bool f_hideWhenFull = true;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private SpriteRenderer f_renderer;

    MaterialPropertyBlock f_matProp;

    private int f_maxValue;

    #endregion

    #region [PrivateVariables]

    private int m_value;
    private int m_showValue;
    private float m_changeTime;

    #endregion

    #region [Properties]

    public bool Visibility {
        set {
            f_renderer.enabled = value;
        }
    }

    #endregion

    #region [Init]

    private void Awake() {
        f_matProp = new MaterialPropertyBlock();
    }

    public void Init(int maxValue, int startValue = -1) {
        f_maxValue = maxValue;
        //f_renderer.enabled = false;

        m_value = maxValue;

        if (startValue != -1) {
            m_showValue = startValue;
        } else {
            m_showValue = maxValue;
        }

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
        if (f_matProp == null) {
            //TODO
            return;
        }

        f_renderer.GetPropertyBlock(f_matProp);
        f_matProp.SetFloat("_Real", m_value / (float)f_maxValue);
        f_matProp.SetFloat("_Show", m_showValue / (float)f_maxValue);
        f_renderer.SetPropertyBlock(f_matProp);

        if (m_value == f_maxValue && m_showValue == f_maxValue) {
            if (f_hideWhenFull) {
                f_renderer.enabled = false;
            }
            return;
        }
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