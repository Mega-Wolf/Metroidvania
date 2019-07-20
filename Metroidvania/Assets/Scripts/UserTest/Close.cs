using UnityEngine;

public class Close : MonoBehaviour {
    
    [SerializeField] private TMPro.TMP_InputField f_input;

    public void Exit() {
        //SceneLoader.Instance.CurrentExperiment.SetFinalText(f_input.text);
        SceneLoader.Instance.finalText = f_input.text;
        Application.Quit();
    }

}