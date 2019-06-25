public class RhinoExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues => throw new System.NotImplementedException();

    #endregion

    #region [Constructors]

    public RhinoExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        //TODO
    }

    #endregion

    #region [Override]

    protected override void AdjustValue() {
        throw new System.NotImplementedException();
    }

    #endregion
}