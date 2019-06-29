public class RhinoExperiment : Experiment {

    #region [Properties]

    public override string[] FeedbackTexts {
        get {
            return
                //new string[] { "Faster", "Slower", "Lower", "Higher", "Shorter", "Longer" },
                //new string[] { "Meteors", "Rhino Health", "Character Health", "Time after the character is hit and can't attack", "Notification time before a meteor", "Meteor Accuracy" });
                new string[] {
                    "Slower Meteors",
                    "Lower Rhino Health",
                    "Higher Character Health",
                    "Shorter time after the character is hit and can't attack",
                    //"Notification time before a meteor", //TODO
                    "Lower Meteor Accuracy"
                };
        }
    }

    #endregion

    #region [Constructors]

    public RhinoExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_text = "Meteors"; //TODO
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
        //TODO
    }

    #endregion
}