public class FrogExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues {
        get {
            return (
                new string[] { "Faster", "Slower", "Lower", "Higher", "Shorter", "Longer" },
                new string[] { "Slime balls", "Frog Health", "Character Health", "Time after the character is hit and can't attack", "Time before a frog pukes", "Slime Accuracy" });
        }
    }

    #endregion

    #region [Constructors]

    public FrogExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_adjective = "Slower";
                f_noun = "Slime balls";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_adjective = "Longer";
                f_noun = "Time before a frog pukes";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_adjective = "Lower";
                f_noun = "Slime Accuracy";
                break;
        }
    }

    #endregion

    #region [Override]

    protected override void AdjustValue() {
        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                Spit.SPEED_FACTOR = (1 - m_currentLevel / 10f);
                FrogControllerState.CAST_TIME = 50;
                Spit.ACCURACY = 0;
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                Spit.SPEED_FACTOR = 1f;
                FrogControllerState.CAST_TIME = (int)(50 * (1 + m_currentLevel / 10f));
                Spit.ACCURACY = 0;
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                Spit.SPEED_FACTOR = 1f;
                FrogControllerState.CAST_TIME = 50;
                Spit.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}