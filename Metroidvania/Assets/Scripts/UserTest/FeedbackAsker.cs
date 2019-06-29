using Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

/// <summary>
/// This part of the code asks whether the player found out what is changing during the sessions
/// </summary>

public class FeedbackAsker : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private TMP_Dropdown f_dropdownNouns;

    [SerializeField] private GameObject f_feedbackRight;
    [SerializeField] private GameObject f_feedbackWrong;

    #endregion

    #region [PublicMethods]

    public void ShowQuestions(Experiment experiment) {
        gameObject.SetActive(true);

        string[] texts = experiment.FeedbackTexts;

        List<OptionData> textODs = new List<OptionData>();

        foreach (string n in texts) {
            textODs.Add(new OptionData(n));
        }

        textODs.Shuffle();

        textODs.Insert(0, new OptionData("Nothing selected"));

        f_dropdownNouns.options = textODs;
    }

    public void FeedbackGiven() {
        gameObject.SetActive(false);

        if (f_dropdownNouns.value > 0) {
            string chosenText = f_dropdownNouns.options[f_dropdownNouns.value].text;

            Experiment exp = SceneLoader.Instance.CurrentExperiment;

            if (chosenText == exp.Text) {
                exp.Realised = true;
                Debug.LogWarning("REALISED correct answer");
                f_feedbackRight.SetActive(true);
                return;
            }
        }

        f_feedbackWrong.SetActive(true);
    }

    public void Skip() {
        f_feedbackRight.SetActive(false);
        f_feedbackWrong.SetActive(false);
        gameObject.SetActive(false);
        SceneLoader.Instance.StartScene(false);
    }

    #endregion

}