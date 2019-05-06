using UnityEngine;


/// <summary>
/// This class manages the game
/// It takes care of using the correct InputManager and that Updates are called if they aren't automatically
/// </summary>
public class GameManager : Singleton<GameManager> {

    //TODO; this should probably made abstract so that there are different GameManagers for playing and replaying
    // No: Playing and repalying means changing the InputManager
    // This thing means changing between edit mode and game mode
    // However, GameMode probably means doing nothing; so I just keep both in here
    // It probably should set the InputManager and maybe load a game (or replay)

    #region [PrivateVariables]

    private int m_frame = 0;

    #endregion

    #region [Properties]

    public int Frame { get { return m_frame; } }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        //TODO; not when paused
        ++m_frame;
    }

    #endregion

}