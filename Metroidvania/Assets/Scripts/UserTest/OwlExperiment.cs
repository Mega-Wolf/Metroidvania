using UnityEngine;

public class OwlExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues {
        get {
            return (
                new string[] { "Faster", "Slower", "Lower", "Higher", "Shorter", "Longer" },
                new string[] { "Feathers", "Owl Health", "Character Health", "Time after the character is hit and can't attack", "Time a feather spins before flying towards the player", "Feather Accuracy" });
        }
    }

    #endregion

    #region [Constructors]

    public OwlExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_adjective = "Slower";
                f_noun = "Feathers";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_adjective = "Longer";
                f_noun = "Time a feather spins before flying towards the player";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_adjective = "Lower";
                f_noun = "Feather Accuracy";
                break;
        }
    }

    #endregion

    #region [PrivateMethods]

    protected override void AdjustValue() {
        Feather.SPEED = 10f;
        Feather.CAST_TIME = 20;
        Feather.ACCURACY = 0;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                Feather.SPEED = 10f * (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                Feather.CAST_TIME = (int)(20 * (1 + m_currentLevel / 10f));
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                Feather.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}