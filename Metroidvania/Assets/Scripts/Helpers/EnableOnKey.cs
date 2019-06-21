using UnityEngine;

public class EnableOnKey : MonoBehaviour {
    
    #region [MemberFields]

    [SerializeField] private MonoBehaviour f_toEnable;

    #endregion

    #region [Updates]

    private void Update() {
        if (Input.anyKeyDown == true) {
            f_toEnable.enabled = true;
            Destroy(gameObject);
        }
    }

    #endregion

}