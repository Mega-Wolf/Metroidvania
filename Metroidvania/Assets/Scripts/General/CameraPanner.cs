using UnityEngine;

public class CameraPanner : MonoBehaviour {

    #region [Physics]

    private void OnTriggerEnter2D(Collider2D other) {
        Consts.Instance.Camera.FollowCam.enabled = false;
        //TODO; disable input (doesn't work due to COntroller stuff)
        Consts.Instance.Camera.CameraMover.MoveCamera(transform.position, () => { });
        //TODO; enable input and then start boss fight
        
        
        GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
    }

    #endregion

}