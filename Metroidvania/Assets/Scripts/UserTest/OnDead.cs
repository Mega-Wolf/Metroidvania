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

        int frames = GameManager.Instance.Frame;
        int health = Consts.Instance.Player == null ? 0 : Consts.Instance.Player.Health.Value;
        int enemyHealthCombined = 0;

        GenericEnemy[] genericEnemys = FindObjectsOfType<GenericEnemy>();
        for (int i = 0; i < genericEnemys.Length; ++i) {
            enemyHealthCombined += genericEnemys[i].Health.Value;
        }

        Debug.LogWarning(frames + " --- " + health + " --- " + enemyHealthCombined);

        SceneLoader.Instance.EndedScene(frames, health, enemyHealthCombined);
    }

}