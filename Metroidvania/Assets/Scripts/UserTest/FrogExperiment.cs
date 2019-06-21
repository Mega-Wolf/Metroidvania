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
        throw new System.NotImplementedException();
    }

    #endregion
}