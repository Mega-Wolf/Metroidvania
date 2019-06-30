public class FrogExperiment : Experiment {

    #region [Properties]

    public override string[] FeedbackTexts {
        get {
            return
                new string[] {
                    "Slower Slime balls",
                    "Lower Frog Health",
                    "Higher Character Health",
                    "Shorter time after the character is hit and can't attack",
                    "Longer time before a frog pukes",
                    "Lower Slime ball Accuracy",
                    "Faster Character"
                };
        }
    }

    #endregion

    #region [Constructors]

    public FrogExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_text = "Slower Slime balls";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_text = "Longer time before a frog pukes";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_text = "Lower Slime ball Accuracy";
                break;
        }
    }

    #endregion

    #region [Override]

    protected override void AdjustValue() {

        Spit.SPEED_FACTOR = 1f;
        FrogControllerState.CAST_TIME = 50;
        Spit.ACCURACY = 0;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                Spit.SPEED_FACTOR = (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                //FrogControllerState.CAST_TIME = (int)(20 * (1 + m_currentLevel / 10f));
                FrogControllerState.CAST_TIME = 50 + 5 * m_currentLevel;
                //TODO
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                Spit.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}