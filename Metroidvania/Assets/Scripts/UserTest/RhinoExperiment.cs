public class RhinoExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues {
        get {
            return (
                new string[] { "Faster", "Slower", "Lower", "Higher", "Shorter", "Longer" },
                new string[] { "Meteors", "Rhino Health", "Character Health", "Time after the character is hit and can't attack", "Notification time before a meteor", "Meteor Accuracy" });
        }
    }

    #endregion

    #region [Constructors]

    public RhinoExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_adjective = "Slower";
                f_noun = "Meteors";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_adjective = "Longer";
                f_noun = "Notification time before a meteor";
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                f_adjective = "Lower";
                f_noun = "Meteor Accuracy";
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