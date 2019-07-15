using UnityEngine;

public class TrainEnded : MonoBehaviour {
    
    #region [PublicMethods]

    public void Retrain() {
        gameObject.SetActive(false);
        SceneLoader.Instance.StartScene(false);
    }

    public void StartTest() {
        gameObject.SetActive(false);
        SceneLoader.Instance.CurrentExperiment.Started = true;
        SceneLoader.Instance.CurrentExperiment.NextTry();
        SceneLoader.Instance.StartScene(false);
    }

    #endregion

}