using UnityEngine;

public class OnDead : MonoBehaviour {

    #region [PrivateVariables]

    private bool m_isQuitting;

    #endregion

    void OnApplicationQuit() {
        m_isQuitting = true;
    }

    private void OnDestroy() {
        if (m_isQuitting) {
            return;
        }

        SceneLoader.Instance.EndedScene();
    }

}