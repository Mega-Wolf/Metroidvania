using UnityEngine;

/// <summary>
/// This part of the code asks whether the player found out what is changing during the sessions
/// </summary>

public class FeedbackAsker : MonoBehaviour {

    public void ShowQuestions(Experiment experiment) {
        string[] adjectives = experiment.FeedbackValues.adjectives;
        string[] nouns = experiment.FeedbackValues.nouns;

        //TODO; set selections
    }
    
}