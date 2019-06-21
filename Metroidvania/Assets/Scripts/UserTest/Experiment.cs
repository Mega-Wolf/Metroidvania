using UnityEngine;

public abstract class Experiment {

    #region [FinalVariables]

    protected SceneLoader.ExaminedVariable f_examinedVariable;
    protected string f_adjective;
    protected string f_noun;

    #endregion

    #region [Constructors]

    public Experiment(SceneLoader.ExaminedVariable examinedVariable) {
        f_examinedVariable = examinedVariable;
    }

    #endregion

    #region [PrivateProperties]

    protected int m_currentLevel { get; private set; }

    #endregion

    #region [Properties]

    public bool Realised { get; set; }
    public bool Started { get; set; }
    public bool Finished {
        get {
            return Realised && m_currentLevel == 0;
        }
    }

    public abstract (string[] adjectives, string[] nouns) FeedbackValues { get; }

    #endregion

    #region [PublicMethods]

    public void NextTry() {
        if (!Started) {
            return;
        }
        if (Realised) {
            --m_currentLevel;
        } else {
            ++m_currentLevel;
        }
        AdjustValue();
        Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
    }

    #endregion

    #region [PrivateMethods]

    protected abstract void AdjustValue();

    #endregion

}