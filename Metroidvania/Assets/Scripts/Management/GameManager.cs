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

}