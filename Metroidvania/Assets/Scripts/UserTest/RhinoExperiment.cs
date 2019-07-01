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
                    "Longer rhino teleportation time between doors",
                    "Slower Rhinos",
                    "Lower Meteor Accuracy",
                    "Faster Character"
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
                f_text = "Longer rhino teleportation time between doors";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_text = "Lower Meteor Accuracy";
                break;
        }
    }

    #endregion

    #region [Override]

    protected override void AdjustValue() {
        BossFightRhino.SPEED = 1f;
        BossFightRhino.DOOR_DELTA = 20;
        DifficultMeteorSpawn.ACCURACY = 0;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                BossFightRhino.SPEED = (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                //BossFightRhino.DOOR_DELTA = (int) (20 * (1 + m_currentLevel / 10f));
                BossFightRhino.DOOR_DELTA = 20 + 5 * m_currentLevel;
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                DifficultMeteorSpawn.ACCURACY = 0.25f * m_currentLevel;
                break;
        }
    }

    #endregion
}