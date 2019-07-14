using UnityEngine;

public class OwlExperiment : Experiment {

    #region [Properties]

    public override string[] FeedbackTexts {
        get {
            return
                new string[] {
                    "Slower Feathers",
                    "Lower Owl Health",
                    "Higher Character Health",
                    "Shorter time after the character is hit and can't attack",
                    "Longer time a feather spins before flying towards the player",
                    "Lower Feather Accuracy",
                    "Slower Owl",
                    "Faster Character",
                    };
        }
    }

    #endregion

    #region [Constructors]

    public OwlExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_text = "Slower Feathers";
                break;
            case SceneLoader.ExaminedVariable.BreakTime:
                f_text = "Longer time a feather spins before flying towards the player";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_text = "Lower Feather Accuracy";
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
            case SceneLoader.ExaminedVariable.BreakTime:
                //Feather.CAST_TIME = (int)(20 * (1 + m_currentLevel / 10f));
                Feather.CAST_TIME = 20 + 5 * m_currentLevel;
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                Feather.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}