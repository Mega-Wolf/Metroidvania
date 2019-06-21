using UnityEngine;

public class TrainEnded : MonoBehaviour {
    
    public void Retrain() {
        SceneLoader.Instance.EndedScene();
    }

    public void StartTest() {
        SceneLoader.Instance.CurrentExperiment.Started = true;
        SceneLoader.Instance.EndedScene();
    }

}