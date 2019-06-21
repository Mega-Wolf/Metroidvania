using UnityEngine;

public class OnDead : MonoBehaviour {
    
    #region [MemberFields]

    [SerializeField] private GameObject f_canvasRetest;
    [SerializeField] private GameObject f_canvasQuestions;

    #endregion

    private void OnDestroy() {
        Experiment exp = SceneLoader.Instance.CurrentExperiment;

        if (!exp.Started) {
            f_canvasRetest.SetActive(true);
            return;
        }

        if (!exp.Realised) {
            f_canvasQuestions.SetActive(true);
            return;
        }

        SceneLoader.Instance.EndedScene();
    }

}