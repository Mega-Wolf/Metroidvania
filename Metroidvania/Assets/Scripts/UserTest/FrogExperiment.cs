public class FrogExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues => throw new System.NotImplementedException();

    #endregion

    #region [Constructors]

    public FrogExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        //TODO
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