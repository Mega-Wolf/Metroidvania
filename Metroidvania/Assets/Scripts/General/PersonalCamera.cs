using UnityEngine;

public class PersonalCamera : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook]
    private SimpleFollowCam f_simpleFollowCam;

    [SerializeField, Autohook]
    private CameraMover f_cameraMover;

    #endregion

    #region [PublicVariables]

    [HideInInspector]
    public Vector2 Velocity;

    #endregion

    #region [Init]

    private void Awake() {
        Consts.Instance.Camera = this;
    }

    #endregion

    #region [Properties]

    public SimpleFollowCam FollowCam { get { return f_simpleFollowCam; } }

    public CameraMover CameraMover { get { return f_cameraMover; } }

    #endregion

}