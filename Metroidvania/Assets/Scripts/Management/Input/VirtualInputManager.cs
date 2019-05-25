using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static InputSave;

/// <summary>
/// Recorded input which is now played back
/// </summary>
public class VirtualInputManager : IInputManager {

    #region [FinalVariables]

    //private Dictionary<string, float> f_axisValues = new Dictionary<string, float>();
    private Dictionary<string, bool> f_buttonValuesLastFrame = new Dictionary<string, bool>();
    private Dictionary<string, bool> f_buttonValues = new Dictionary<string, bool>();

    private Dictionary<string, int> f_lastDown = new Dictionary<string, int>();

    private InputSave f_inputSave;

    private string[] f_buttons;

    #endregion

    #region [PrivateVariables]

    private int m_positionInLayer;
    private int m_layer = 0;
    private List<InputData> m_currentLayer;

    #endregion

    #region [Constructors]

    public VirtualInputManager(InputSave inputSave, string[] buttons) {
        f_inputSave = inputSave;
        f_buttons = buttons;

        Clear();
    }

    #endregion

    #region [Updates]

    public void HandleUpdate() {

        foreach (string button in f_buttons) {
            f_buttonValuesLastFrame[button] = f_buttonValues[button];
        }

        while (m_positionInLayer < m_currentLayer.Count) {
            InputData id = m_currentLayer[m_positionInLayer];

            int frameNumber = InputManager.Instance.ReplayFrame;

            if (frameNumber == id.Frame) {
                SetButton(id.Button, id.Value);
                ++m_positionInLayer;
            } else {
                break;
            }
        }
    }

    #endregion

    #region [PublicMethods]

    public void Clear() {
        foreach (string button in f_buttons) {
            f_buttonValues[button] = false;
            f_buttonValuesLastFrame[button] = false;
            f_lastDown[button] = int.MinValue;
        }
    }

    #region [Replays]

    public void StartReplay() {
        m_layer = 0;
        m_positionInLayer = 0;
        Clear();
        m_currentLayer = f_inputSave.GetLayer(m_layer);
    }

    public void PauseReplay() {
        ++m_layer;
        m_positionInLayer = 0;
        Clear();
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

    public bool GetButtonDown(string virtualKey, InputManager.EDelayType delayType) {
        if (f_buttonValues[virtualKey] && !f_buttonValuesLastFrame[virtualKey]) {
            f_lastDown[virtualKey] = int.MinValue;
            return true;
        }

        if ((delayType == InputManager.EDelayType.Always || (delayType == InputManager.EDelayType.OnlyWhenDown && f_buttonValues[virtualKey]) || (delayType == InputManager.EDelayType.OnlyWhenUp && !f_buttonValues[virtualKey])) && f_lastDown[virtualKey] + InputManager.LATE_DOWN >= GameManager.Instance.Frame) {
            f_lastDown[virtualKey] = int.MinValue;
            return true;
        }

        return false;

        //return f_buttonValues[virtualKey] && !f_buttonValuesLastFrame[virtualKey];
    }

    public bool GetButtonUp(string virtualKey) {
        return !f_buttonValues[virtualKey] && f_buttonValuesLastFrame[virtualKey];
    }

    #endregion

    #endregion
}
