using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static InputSave;

/// <summary>
/// The actual input, but useable in FixedUpdate
/// Therefore, Input.GetButtonUp/Down will not work, since it is updated in Update
/// </summary>
public class VirtualInputManager : IInputManager {

    #region [MemberFields]

    [SerializeField]
    private string[] f_buttons;

    #endregion

    #region [FinalVariables]

    //private Dictionary<string, float> f_axisValues = new Dictionary<string, float>();
    private Dictionary<string, bool> f_buttonValuesLastFrame = new Dictionary<string, bool>();
    private Dictionary<string, bool> f_buttonValues = new Dictionary<string, bool>();

    private InputSave f_inputSave;

    #endregion

    #region [PrivateVariables]

    private int m_positionInLayer;
    private int m_layer = 0;
    private List<InputData> m_currentLayer;

    #endregion

    #region [Constructors]

    public VirtualInputManager(InputSave inputSave) {
        f_inputSave = inputSave;

        Clear();
    }

    #endregion

    #region [Updates]

    public void HandleUpdate() {

        while (m_positionInLayer < m_currentLayer.Count) {
            InputData id = m_currentLayer[m_positionInLayer];

            int frameNumber = InputManager.Instance.ReplayFrame;

            if (frameNumber == id.Frame) {
                SetButton(id.Button, id.Value);
                ++m_positionInLayer;
            }
        }

        foreach (string button in f_buttons) {
            f_buttonValuesLastFrame[button] = f_buttonValues[button];
        }
    }

    #endregion

    #region [PublicMethods]

    public void Clear() {
        foreach (string button in f_buttons) {
            f_buttonValues[button] = false;
            f_buttonValuesLastFrame[button] = false;
        }
    }

    #region [Replays]

    public void StartReplay() {
        m_layer = 0;
        m_positionInLayer = 0;
        m_currentLayer = f_inputSave.GetLayer(m_layer);
    }

    public void PauseReplay() {
        ++m_layer;
        m_positionInLayer = 0;
        m_currentLayer = f_inputSave.GetLayer(m_layer);
    }

    public void StopReplay() {
        //TODO; don't know; abort probably
    }

    #endregion

    #endregion

    //TODO; It seems like the Setters won't be needed since the data is read from in here
    #region [Setter]

    #region [Buttons]

    public void SetButton(string virtualKey, bool pressed) {
        f_buttonValues[virtualKey] = pressed;
    }

    #endregion

    #endregion

    #region [Getter]

    #region [Buttons]

    public bool GetButton(string virtualKey) {
        return f_buttonValues[virtualKey];
    }

    public bool GetButtonDown(string virtualKey) {
        return f_buttonValues[virtualKey] && !f_buttonValuesLastFrame[virtualKey];
    }

    public bool GetButtonUp(string virtualKey) {
        return !f_buttonValues[virtualKey] && f_buttonValuesLastFrame[virtualKey];
    }

    #endregion

    #endregion
}
