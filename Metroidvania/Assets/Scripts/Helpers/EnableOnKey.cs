using UnityEngine;

public class EnableOnKey : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Behaviour[] f_toEnable;

    #endregion

    #region [FinalVariables]

    private float f_startTime;

    #endregion

    #region [Init]

    private void Awake() {
        f_startTime = Time.time;
    }

    #endregion

    #region [Updates]

    private void Update() {
        if (Time.time < f_startTime + 1f) {
            return;
        }
        
        //if (Input.anyKeyDown == true) {
        if (Input.GetButtonDown("Dash") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Left") || Input.GetButtonDown("Right") || Input.GetButtonDown("Up") || Input.GetButtonDown("Down") || Input.GetButtonDown("Fight") || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f) {
            for (int i = 0; i < f_toEnable.Length; ++i) {
                f_toEnable[i].enabled = true;
            }
            Destroy(gameObject);

            if (SceneLoader.Instance) {
                SceneLoader.Instance.WinLoseText.text = "";
            }
        }
    }

    #endregion

}