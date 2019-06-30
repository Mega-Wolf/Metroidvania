public class RhinoExperiment : Experiment {

    #region [Properties]

    public override string[] FeedbackTexts {
        get {
            return
                new string[] {
                    "Slower Meteors",
                    "Lower Rhino Health",
                    "Higher Character Health",
                    "Shorter time after the character is hit and can't attack",
                    "Slower Rhinos",
                    "Lower Meteor Accuracy",
                };
        }
    }

    #endregion

    #region [Constructors]

    public RhinoExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_text = "Slower Rhinos";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_text = "Notification time before a meteor"; //TODO
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_text = "Meteor Accuracy"; //TODO
                break;
        }
    }

    #endregion

    #region [Override]

    protected override void AdjustValue() {
        BossFightRhino.SPEED = 1f;
        BossFightRhino.DOOR_DELTA = 20;
        //Feather.ACCURACY = 0;

        //TODO

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                BossFightRhino.SPEED = (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                //BossFightRhino.DOOR_DELTA = (int) (20 * (1 + m_currentLevel / 10f));
                BossFightRhino.DOOR_DELTA = 20 + 5 * m_currentLevel;
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                //Feather.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}