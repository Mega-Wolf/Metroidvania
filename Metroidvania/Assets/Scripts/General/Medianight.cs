using UnityEngine;
using UnityEngine.SceneManagement;

public class Medianight : Singleton<Medianight> {

    #region [MemberFields]

    [SerializeField] private GameObject f_selection;

    #endregion

    #region [PrivateVariables]

    private bool m_isQuitting = false;

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.Escape)) {
            if (SceneManager.sceneCount > 1) {
                EndedScene();
            }
        }
    }

    #endregion

    #region [PublicMethods]

    public void LoadScene(string sceneName) {
        f_selection.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void EndedScene() {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        if (ao == null) {
            f_selection.SetActive(true);
            FindObjectOfType<PersonalCamera>().FollowCam.enabled = false;
            FindObjectOfType<PersonalCamera>().transform.position = new Vector3(0, 0, -10);
        } else {
            ao.completed += (AsyncOperation a) => {
                if (m_isQuitting) {
                    return;
                }

                f_selection.SetActive(true);
                FindObjectOfType<PersonalCamera>().FollowCam.enabled = false;
                FindObjectOfType<PersonalCamera>().transform.position = new Vector3(0, 0, -10);
            };
        }
    }

    #endregion

    private void OnApplicationQuit() {
        m_isQuitting = true;
    }

}