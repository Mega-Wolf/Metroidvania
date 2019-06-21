public class OwlExperiment : Experiment {

    #region [Properties]

    public override (string[] adjectives, string[] nouns) FeedbackValues {
        get {
            return (
                new string[] { /* TODO */ },
                new string[] { /* TODO */ });
        }
    }

    #endregion

    #region [Constructors]

    public OwlExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {
        switch (examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                f_adjective = "Faster";
                f_noun = "Feathers";
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                f_adjective = "Shorter";
                f_noun = "casting time (the time a feather spins before starting flying towards the player)";
                break;
            case SceneLoader.ExaminedVariable.Health:
                f_adjective = "Lower";
                f_noun = "Owl Health";
                break;
        }
    }

    #endregion

    #region [PrivateMethods]

    protected override void AdjustValue() {
        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                //TODO; set value
                break;
            case SceneLoader.ExaminedVariable.CastTime:
                //TODO; set value
                break;
            case SceneLoader.ExaminedVariable.Health:
                //TODO; set value
                break;
        }
    }

    #endregion
}