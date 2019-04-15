using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The actual input, but useable in FixedUpdate
/// Therefore, Input.GetButtonUp/Down will not work, since it is updated in Update
/// </summary>
public class VirtualInputManager : InputManager {

    #region [MemberFields]

    [SerializeField]
    private string[] f_buttons;

    #endregion

    #region [FinalVariables]

    //private Dictionary<string, float> f_axisValues = new Dictionary<string, float>();
    private Dictionary<string, bool> f_buttonValuesLastFrame = new Dictionary<string, bool>();
    private Dictionary<string, bool> f_buttonValues = new Dictionary<string, bool>();

    #endregion

    #region [Init]

    protected override void Awake() {
        base.Awake();

        Clear();
    }

    #endregion

    #region [Updates]

    public void FixedUpdate() {
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

    #endregion

    #region [Setter]

    #region [Buttons]

    public void SetButton(string virtualKey, bool pressed) {
        f_buttonValues[virtualKey] = pressed;
    }

    #endregion

    #endregion

    #region [Getter]

    #region [Buttons]

    public override bool GetButton(string virtualKey) {
        return f_buttonValues[virtualKey];
    }

    public override bool GetButtonDown(string virtualKey) {
        return f_buttonValues[virtualKey] && !f_buttonValuesLastFrame[virtualKey];
    }

    public override bool GetButtonUp(string virtualKey) {
        return !f_buttonValues[virtualKey] && f_buttonValuesLastFrame[virtualKey];
    }

    #endregion

    // #region [Joysticks]

    // public override float GetAxis(string virtualAxis) {
    // 	if (Input.GetAxis(virtualAxis) == 0) {
    // 		return 0;
    // 	}

    //     //TODO; I migth look at that again

    // 	// Only returns -1, 0 or 1 (=> no gravity or sensitivity)
    // 	// That way the AI will have the same possible inputs as the player

    // 	return Mathf.Sign(Input.GetAxis(virtualAxis));
    // 	// return Input.GetAxis(virtualAxis);
    // }

    // #endregion

    #endregion
}
