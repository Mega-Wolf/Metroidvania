using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The actual input, but useable in FixedUpdate
/// Therefore, Input.GetButtonUp/Down will not work, since it is updated in Update
/// </summary>
public class HardwareInputManager : IInputManager {

    #region [FinalVariables]

    //private Dictionary<string, float> f_axisValues = new Dictionary<string, float>();
    private Dictionary<string, bool> f_buttonValuesLastFrame = new Dictionary<string, bool>();
    private Dictionary<string, bool> f_buttonValues = new Dictionary<string, bool>();

    private InputSave f_inputSave;

    private string[] f_buttons;

    #endregion

    #region [Constructors]

    public HardwareInputManager(InputSave inputSave, string[] buttons) {
        f_inputSave = inputSave;
        f_buttons = buttons;

        foreach (string button in f_buttons) {
            f_buttonValues[button] = false;
            f_buttonValuesLastFrame[button] = false;
        }
    }

    #endregion

    #region [Updates]

    public void HandleUpdate() {
        foreach (string button in f_buttons) {
            bool newValue = Input.GetButton(button);

            if (newValue != f_buttonValues[button]) {
                f_inputSave.AddButtonInput(button, newValue);
                //TODO; ignore when menu
            }

            f_buttonValuesLastFrame[button] = f_buttonValues[button];
            f_buttonValues[button] = newValue;
        }
    }

    #endregion

    #region [PublicMethods]

    public void StartReplay() {
        //TODO; I have no idea what I should do here
    }

    public void PauseReplay() {
        f_inputSave.Split();
    }

    public void StopReplay() {
        //TODO; I have no idea what I should do here; save?
    }

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
