using UnityEngine;

public class EnableOnKey : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Behaviour[] f_toEnable;

    #endregion

    #region [Updates]

    private void Update() {
        //if (Input.anyKeyDown == true) {
        if (Input.GetButtonDown("Dash") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Left") || Input.GetButtonDown("Right") || Input.GetButtonDown("Up") || Input.GetButtonDown("Down") || Input.GetButtonDown("Fight") || Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0) {
            for (int i = 0; i < f_toEnable.Length; ++i) {
                f_toEnable[i].enabled = true;
            }
            Destroy(gameObject);
        }
    }

    #endregion

}