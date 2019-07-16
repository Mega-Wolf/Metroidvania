using UnityEngine;

public class FrogExperiment : Experiment {

    #region [Constructors]

    public FrogExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) {}

    #endregion

    #region [Override]

    protected override void AdjustValue() {

        Spit.SPEED_FACTOR = 1f;
        FrogControllerState.CAST_TIME = 50;
        FrogControllerState.POST_IDLE = 0;
        Spit.ACCURACY = 0;
        GenericEnemy.INITIAL_HEALTH = 8;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                Spit.SPEED_FACTOR = (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.BreakTime:
                FrogControllerState.POST_IDLE = (int) ((FrogControllerState.CAST_TIME + FrogControllerState.PUKE_TIME + FrogControllerState.END_IDLE) / (1 - m_currentLevel / 10f) - (FrogControllerState.CAST_TIME + FrogControllerState.PUKE_TIME + FrogControllerState.END_IDLE));
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                //Spit.ACCURACY = 0.25f * m_currentLevel;
                //TODO
                break;
            case SceneLoader.ExaminedVariable.Health:
                GenericEnemy.INITIAL_HEALTH = (int) (8 * (1 - m_currentLevel));
                break;
        }

        Debug.Log(Spit.SPEED_FACTOR);
        Debug.Log(FrogControllerState.CAST_TIME);
        Debug.Log(FrogControllerState.POST_IDLE);
        Debug.Log(Spit.ACCURACY);
        Debug.Log(GenericEnemy.INITIAL_HEALTH);
    }

    #endregion
}