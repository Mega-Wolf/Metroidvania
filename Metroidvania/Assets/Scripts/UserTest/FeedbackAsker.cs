using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

/// <summary>
/// This part of the code asks whether the player found out what is changing during the sessions
/// </summary>

public class FeedbackAsker : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Dropdown f_dropdownNouns;
    [SerializeField] private Dropdown f_dropdownAdjectives;

    #endregion

    public void ShowQuestions(Experiment experiment) {
        gameObject.SetActive(true);

        string[] adjectives = experiment.FeedbackValues.adjectives;
        string[] nouns = experiment.FeedbackValues.nouns;

        List<OptionData> adjs = new List<OptionData>();
        List<OptionData> nns = new List<OptionData>();

        foreach (string a in adjectives) {
            adjs.Add(new OptionData(a));
        }

        foreach (string n in nouns) {
            nns.Add(new OptionData(n));
        }

        f_dropdownAdjectives.options = adjs;
        f_dropdownNouns.options = nns;
    }

    public void FeedbackGiven() {
        //TODO check if realised
        gameObject.SetActive(false);
        SceneLoader.Instance.StartScene(false);
    }

}