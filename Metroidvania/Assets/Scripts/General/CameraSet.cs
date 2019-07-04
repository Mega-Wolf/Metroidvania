using UnityEngine;

public class CameraSet : MonoBehaviour {
    
    private void Start() {
        Consts.Instance.Camera = FindObjectOfType<Camera>().GetComponent<PersonalCamera>();
        Consts.Instance.Camera.transform.position = Consts.Instance.Player.transform.position + 3 * Vector3.up + 10 * Vector3.back;
        Consts.Instance.Camera.FollowCam.Followed = Consts.Instance.Player.transform;
        Consts.Instance.Camera.FollowCam.enabled = true;
    }

}