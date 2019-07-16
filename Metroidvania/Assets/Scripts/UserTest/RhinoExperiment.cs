using UnityEngine;

public class RhinoExperiment : Experiment {

    #region [Constructors]

    public RhinoExperiment(SceneLoader.ExaminedVariable examinedVariable) : base(examinedVariable) { }

    #endregion

    #region [Override]

    protected override void AdjustValue() {
        BossFightRhino.SPEED = 1f;
        BossFightRhino.DOOR_DELTA = 20;
        DifficultMeteorSpawn.ACCURACY = 0;
        DifficultMeteorSpawn.OFFSET = 27;
        GenericEnemy.INITIAL_HEALTH = 8;

        switch (f_examinedVariable) {
            case SceneLoader.ExaminedVariable.AttackSpeed:
                BossFightRhino.SPEED = (1 - m_currentLevel / 10f);
                break;
            case SceneLoader.ExaminedVariable.BreakTime:
                DifficultMeteorSpawn.OFFSET = (int) (27 / (1 - m_currentLevel / 10f));
                break;
            case SceneLoader.ExaminedVariable.Accuracy:
                //DifficultMeteorSpawn.ACCURACY = 0.25f * m_currentLevel;
                //TODO
                break;
            case SceneLoader.ExaminedVariable.Health:
                GenericEnemy.INITIAL_HEALTH = (int)(8 * (1 - m_currentLevel / 10f));
                break;
        }

        Debug.Log(BossFightRhino.SPEED);
        Debug.Log(BossFightRhino.DOOR_DELTA);
        Debug.Log(DifficultMeteorSpawn.ACCURACY);
        Debug.Log(DifficultMeteorSpawn.OFFSET);
        Debug.Log(GenericEnemy.INITIAL_HEALTH);
    }

    #endregion
}