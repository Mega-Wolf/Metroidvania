using UnityEngine;

public class OwlExperiment : Experiment {

    #region [Constructors]

    public OwlExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) { }

    #endregion

    #region [PrivateMethods]

    protected override void AdjustValue() {
        Feather.SPEED = 10f;

        Feather.CAST_TIME = 20;
        FeatherFight.POST_IDLE = 0;

        Feather.ACCURACY = 0;

        GenericEnemy.INITIAL_HEALTH = 12;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                Feather.SPEED = 10f * (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.BreakTime:
                FeatherFight.POST_IDLE = (int)((FeatherFight.PRE_IDLE + FeatherFight.PREPARATION) / (1 - m_currentLevel / 10f) - (FeatherFight.PRE_IDLE + FeatherFight.PREPARATION));
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                Feather.ACCURACY = 0.25f * m_currentLevel;
                //TODO
                break;
            case SceneLoader.ExaminedVariable.Health:
                GenericEnemy.INITIAL_HEALTH = (int)(8 * (1 - m_currentLevel / 10f));
                break;
        }

        Debug.Log(Feather.SPEED);
        Debug.Log(Feather.CAST_TIME);
        Debug.Log(FeatherFight.POST_IDLE);
        Debug.Log(Feather.ACCURACY);
        Debug.Log(GenericEnemy.INITIAL_HEALTH);
    }

    #endregion
}