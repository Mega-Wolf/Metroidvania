using UnityEngine;


/// <summary>
/// Interface for the Virtual and Hardware InputManager
/// </summary>
public interface IInputManager {
    void StartReplay();
    void PauseReplay();
    void StopReplay();

    void HandleUpdate();

    bool GetButton(string virtualKey);
    bool GetButtonDown(string virtualKey, InputManager.EDelayType delayType);
    bool GetButtonUp(string virtualKey);
}

/// <summary>
/// This class combined all the different input types and makes it possible to play the game in normal and edit mode
/// It is also possible to save and reload inputs here
/// </summary>
public class InputManager : Singleton<InputManager> {

    #region [Consts]

    public const int LATE_DOWN = 5;

    #endregion

    #region [Types]

    private enum EInputManager {
        Hardware,
        Virtual
    }

    public enum EDelayType {
        None, // when walking left / right
        OnlyWhenDown,   // when jumping, since that has to be done in order to be effective
        OnlyWhenUp, // dunno
        Always // when casting a spell, I don't care if I already released it
    }

    #endregion

    #region [MemberFields]

    [SerializeField]
    private EInputManager f_eInputManager;

    [SerializeField]
    private InputSaveSO f_inputSaveSO;

    [SerializeField]
    private string[] f_buttons;

    #endregion

    #region [PrivateVariables]

    private int m_replayFrame = 0;
    private bool m_isPaused = false;

    private IInputManager f_inputManager;

    #endregion

    #region [Properties]

    public bool IsPaused { get { return m_isPaused; } }
    public int ReplayFrame { get { return m_replayFrame; } }
    public bool IgnoreInput { get; set; }

    #endregion

    #region [Init]

    protected override void Awake() {
        base.Awake();
        switch (f_eInputManager) {
            case EInputManager.Hardware: {
                    InputSave inputSave = new InputSave();
                    f_inputSaveSO.InputSave = inputSave;
                    f_inputManager = new HardwareInputManager(inputSave, f_buttons);
                }

                break;
            case EInputManager.Virtual:
                f_inputManager = new VirtualInputManager(f_inputSaveSO.InputSave, f_buttons);
                f_inputManager.StartReplay();
                break;
        }
    }

    #endregion

    #region [Updates]

    protected virtual void FixedUpdate() {
        if (!m_isPaused) {
            ++m_replayFrame;
        }
        f_inputManager.HandleUpdate();
    }

    #endregion

    #region [PublicMethods]

    #region [Recording]

    /// <summary>
    /// Starts recording or replaying a replay
    /// </summary>
    public void StartReplay() {
        m_replayFrame = 0;
        m_isPaused = false;
        f_inputManager.StartReplay();
    }

    /// <summary>
    /// Pauses recording or replaying a replay
    /// </summary>
    public void PauseReplay() {
        m_isPaused = true;
        f_inputManager.PauseReplay();
    }

    /// <summary>
    /// Stops recording or replaying a replay
    /// </summary>
    public void StopReplay() {
        f_inputManager.StopReplay();
    }

    #endregion

    #region [Buttons]

    /// <summary>
    /// Returns true if the virtual button with the given name is pressed
    /// </summary>
    public bool GetButton(string virtualKey, bool ignoreIgnore = false) {
        if (GameManager.Instance.Frame == 0) {
            return false;
        }
        return (!IgnoreInput || ignoreIgnore) && f_inputManager.GetButton(virtualKey);
    }

    /// <summary>
    /// Returns true if the virtual button with the given name was pressed this frame
    /// </summary>
    public bool GetButtonDown(string virtualKey, EDelayType delayType = EDelayType.None, bool ignoreIgnore = false) {
        if (GameManager.Instance.Frame == 0) {
            return false;
        }
        return (!IgnoreInput || ignoreIgnore) && f_inputManager.GetButtonDown(virtualKey, delayType);
    }

    /// <summary>
    /// Returns true if the virtual button with the given name was released this frame
    /// </summary>
    public bool GetButtonUp(string virtualKey, bool ignoreIgnore = false) {
        if (GameManager.Instance.Frame == 0) {
            return false;
        }
        return (!IgnoreInput || ignoreIgnore) && f_inputManager.GetButtonUp(virtualKey);
    }

    #endregion

    #endregion

}