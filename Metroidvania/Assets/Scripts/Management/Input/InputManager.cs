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
    bool GetButtonDown(string virtualKey);
    bool GetButtonUp(string virtualKey);
}

/// <summary>
/// This class combined all the different input types and makes it possible to play the game in normal and edit mode
/// It is also possible to save and reload inputs here
/// </summary>
public abstract class InputManager : Singleton<InputManager> {

    #region [PrivateVariables]

    private int m_replayFrame = 0;
    private bool m_isPaused = false;

    private IInputManager f_inputManager;

    #endregion

    #region [Properties]

    public bool IsPaused { get { return m_isPaused; } }
    public int ReplayFrame { get { return m_replayFrame; } }

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
    public bool GetButton(string virtualKey) {
        return f_inputManager.GetButton(virtualKey);
    }

    /// <summary>
    /// Returns true if the virtual button with the given name was pressed this frame
    /// </summary>
    public bool GetButtonDown(string virtualKey) {
        return f_inputManager.GetButtonDown(virtualKey);
    }

    /// <summary>
    /// Returns true if the virtual button with the given name was released this frame
    /// </summary>
    public bool GetButtonUp(string virtualKey) {
        return f_inputManager.GetButtonUp(virtualKey);
    }

    #endregion

    #endregion

}