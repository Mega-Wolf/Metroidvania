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

    private Dictionary<string, int> f_lastDown = new Dictionary<string, int>();

    private InputSave f_inputSave;

    private string[] f_buttons;

    private bool[] f_fightBetweenValues = new bool[1];

    #endregion

    #region [Constructors]

    public HardwareInputManager(InputSave inputSave, string[] buttons) {
        f_inputSave = inputSave;
        f_buttons = buttons;

        Clear();
    }

    #endregion

    #region [Updates]

    public void HandleUpdate() {

        //TODO; non hacky version




        foreach (string button in f_buttons) {
            bool newValue;

            if (button == "Fight") {
                newValue = f_fightBetweenValues[f_fightBetweenValues.Length - 1];
                for (int i = f_fightBetweenValues.Length; --i >= 1;) {
                    f_fightBetweenValues[i] = f_fightBetweenValues[i - 1];
                }
                f_fightBetweenValues[0] = Input.GetButton(button);
            } else {
                switch (button) {
                    case "Left": {
                            if (Input.GetAxisRaw("Horizontal") < 0) {
                                newValue = true;
                            } else {
                                newValue = Input.GetButton(button);
                            }
                            break;
                        }
                    case "Right": {
                            if (Input.GetAxisRaw("Horizontal") > 0) {
                                newValue = true;
                            } else {
                                newValue = Input.GetButton(button);
                            }
                            break;
                        }
                    case "Up": {
                            if (Input.GetAxisRaw("Vertical") > 0) {
                                newValue = true;
                            } else {
                                newValue = Input.GetButton(button);
                            }
                            break;
                        }
                    case "Down": {
                            if (Input.GetAxisRaw("Vertical") < 0) {
                                newValue = true;
                            } else {
                                newValue = Input.GetButton(button);
                            }
                            break;
                        }
                    default: {
                            newValue = Input.GetButton(button);
                            break;
                        }
                }
            }

            if (newValue != f_buttonValues[button]) {
                f_inputSave.AddButtonInput(button, newValue);
                //TODO; ignore when menu
                if (newValue) {
                    // remember when this happened
                    f_lastDown[button] = GameManager.Instance.Frame;
                }
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
        Clear();
        f_inputSave.Split();
    }

    public void StopReplay() {
        //TODO; I have no idea what I should do here; save?
    }

    #endregion

    #region [PrivateMethods]

    private void Clear() {
        foreach (string button in f_buttons) {
            f_buttonValues[button] = false;
            f_buttonValuesLastFrame[button] = false;
            f_lastDown[button] = int.MinValue;
        }
    }

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
