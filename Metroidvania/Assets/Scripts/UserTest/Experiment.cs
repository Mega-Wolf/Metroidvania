using UnityEngine;

public abstract class Experiment {

    #region [FinalVariables]

    protected SceneLoader.ExaminedVariable f_examinedVariable;
    protected string f_adjective;
    protected string f_noun;

    #endregion

    #region [PrivateVariables]

    private int m_endCounter;

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

    public string Noun { get { return f_noun; } }
    public string Adjective { get { return f_adjective; } }

    private bool _realised = false;
    public bool Realised {
        get {
            return _realised;
        }
        set {
            _realised = value;
            m_currentLevel -= 2;
            m_endCounter = 1;
            AdjustValue();
            Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
        }
    }
    public bool Started { get; set; }
    public bool Finished {
        get {
            return Realised && m_endCounter == 6 + 1;
        }
    }

    public abstract (string[] adjectives, string[] nouns) FeedbackValues { get; }

    public int CurrentLevel { get { return m_currentLevel; } }

    #endregion

    #region [PublicMethods]

    public void NextTry() {
        if (!Started) {
            return;
        }
        // if (Realised) {
        //     --m_currentLevel;
        // } else {
        //     ++m_currentLevel;
        // }

        if (!Realised) {
            ++m_currentLevel;
        } else {
            if (m_endCounter == 3) {
                m_currentLevel = 0;
            }
            ++m_endCounter;
        }

        AdjustValue();
        Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
    }

    #endregion

    #region [PrivateMethods]

    protected abstract void AdjustValue();

    #endregion

}