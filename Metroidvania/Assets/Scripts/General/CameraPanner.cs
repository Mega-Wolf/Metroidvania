using UnityEngine;

public class CameraPanner : MonoBehaviour {

    #region [Physics]

    private void OnTriggerEnter2D(Collider2D other) {
        Consts.Instance.Camera.FollowCam.enabled = false;
        Consts.Instance.Camera.CameraMover.MoveCamera(transform.position, () => {
            transform.GetChild(0).gameObject.SetActive(true);
        });        

        GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
    }

    #endregion

}