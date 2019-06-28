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
    [SerializeField] private TMP_Dropdown f_dropdownAdjectives;

    [SerializeField] private GameObject f_feedbackRight;
    [SerializeField] private GameObject f_feedbackWrong;

    #endregion

    #region [PublicMethods]

    public void ShowQuestions(Experiment experiment) {
        gameObject.SetActive(true);

        string[] adjectives = experiment.FeedbackValues.adjectives;
        string[] nouns = experiment.FeedbackValues.nouns;

        List<OptionData> adjs = new List<OptionData>();
        List<OptionData> nns = new List<OptionData>();

        List<TMP_Dropdown.OptionData> t = new List<TMP_Dropdown.OptionData>();

        adjs.Shuffle();
        nns.Shuffle();

        adjs.Add(new OptionData("Nothing selected"));
        foreach (string a in adjectives) {
            adjs.Add(new OptionData(a));
        }

        nns.Add(new OptionData("Nothing selected"));
        foreach (string n in nouns) {
            nns.Add(new OptionData(n));
        }

        f_dropdownAdjectives.options = adjs;
        f_dropdownNouns.options = nns;
    }

    public void FeedbackGiven() {
        gameObject.SetActive(false);

        if (f_dropdownNouns.value > 0 && f_dropdownAdjectives.value > 0) {
            string chosenNoun = f_dropdownNouns.options[f_dropdownNouns.value].text;
            string chosenAdjective = f_dropdownAdjectives.options[f_dropdownAdjectives.value].text;

            Experiment exp = SceneLoader.Instance.CurrentExperiment;

            if (chosenNoun == exp.Noun && chosenAdjective == exp.Adjective) {
                exp.Realised = true;
                Debug.LogWarning("REALISED");
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