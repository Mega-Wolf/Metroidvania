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

    #region [MemberFields]

    [SerializeField]
    [Range(0f, 10f)]
    private float m_speed = 1;

    #endregion

    #region [PrivateVariables]

    private int m_frame = 0;

    #endregion

    #region [Properties]

    public int Frame { get { return m_frame; } }

    #endregion

    // #region [Init]

    // private void Start() {
    //     Consts.Instance.Camera = FindObjectOfType<PersonalCamera>();
    //     Consts.Instance.Camera.FollowCam.Followed = FindObjectOfType<Player>().transform;
    //     Consts.Instance.Camera.FollowCam.enabled = true;
    // }

    // private void OnDisable() {
    //     Consts.Instance.Camera.FollowCam.enabled = false;
    // }

    // #endregion

    #region [Updates]

    private void FixedUpdate() {
        //TODO; not when paused
        //T** The InputManager also has a pause funtion, but that does something differently
        //T** However, that should be remembered, because the InputManager MUST NOT be updated when the game is paused


        if (Input.GetKey(KeyCode.LeftShift)) {
            Time.timeScale = 10;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            Time.timeScale = 3;
        } else {
            Time.timeScale = m_speed;
        }

        ++m_frame;
    }

    #endregion

}