using UnityEngine;

/// <summary>
/// This class combined all the different input types and makes it possible to play the game in normal and edit mode
/// Theoretically it is also possible to save and reload inputs here
/// </summary>
public abstract class InputManager : Singleton<InputManager> {

    #region [PrivateVariables]

    #endregion

    #region [PublicMethods]

    #region [Buttons]

    /// <summary>
    /// Returns true if the virtual button with the given name is pressed
    /// </summary>
    public abstract bool GetButton(string virtualKey);

    /// <summary>
    /// Returns true if the virtual button with the given name was pressed this frame
    /// </summary>
    public abstract bool GetButtonDown(string virtualKey);

    /// <summary>
    /// Returns true if the virtual button with the given name was released this frame
    /// </summary>
    public abstract bool GetButtonUp(string virtualKey);

    #endregion

    // #region [Axis]

    // /// <summary>
    // /// Gets the value of the virtual axis with the given name
    // /// </summary>
    // public abstract float GetAxis(string virtualAxis);

    // #endregion

    #endregion

}