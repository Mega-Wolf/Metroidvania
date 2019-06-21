using UnityEngine;

public class EnableOnKey : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Behaviour[] f_toEnable;

    #endregion

    #region [Updates]

    private void Update() {
        if (Input.anyKeyDown == true) {
            for (int i = 0; i < f_toEnable.Length; ++i) {
                f_toEnable[i].enabled = true;
            }
            Destroy(gameObject);
        }
    }

    #endregion

}