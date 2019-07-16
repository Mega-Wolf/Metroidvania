using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadioFeedback : MonoBehaviour {
    
    [SerializeField] private Toggle f_toggle1;
    [SerializeField] private Toggle f_toggle2;
    [SerializeField] private Toggle f_toggle3;
    [SerializeField] private Toggle f_toggle4;
    [SerializeField] private Toggle f_toggle5;

    [SerializeField] private TMP_Text f_text;

    private int state = 0;

    public void ShowAfterStage1() {
        state = 1;
        f_toggle3.isOn = true;
        f_text.text = "Please now rate the difficulty of the level you played from 1 (very easy) to 5 (very difficult). Afterwards you will play the same level again, however, something will change to adjust the difficulty.";
    }

    public void ShowAfterStage2() {
        state = 2;
        f_toggle3.isOn = true;
        f_text.text = "Please now rate the difficulty of the level you played from 1 (very easy) to 5 (very difficult). Only rate the difficulty of the level AFTER the last time you had to rate it.";
    }

    public void PressButton() {
        int rating = 3;
        if (f_toggle1.isOn) {
            rating = 1;
        }
        if (f_toggle2.isOn) {
            rating = 2;
        }
        if (f_toggle3.isOn) {
            rating = 3;
        }
        if (f_toggle4.isOn) {
            rating = 4;
        }
        if (f_toggle5.isOn) {
            rating = 5;
        }

        SceneLoader.Instance.CurrentExperiment.RateAfterState(state, rating);
        SceneLoader.Instance.StartScene(state == 2);

        gameObject.SetActive(false);
    }

}