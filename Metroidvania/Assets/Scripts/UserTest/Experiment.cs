#define CHANGING

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Experiment {

    private const int TEST_AMOUNT = 10;

    #region [Types]

    public class ExperimentData {

        #region [FinalVariables]

        private SceneLoader.ExaminedVariable f_examinedVariable;
        private List<(int currentLevel, int frames, int characterHealth, int combinedEnemyHealth)> f_data = new List<(int, int, int, int)>();

        #endregion

        #region [Constructors]

        public ExperimentData(SceneLoader.ExaminedVariable examinedVariable) {
            f_examinedVariable = examinedVariable;
        }

        #endregion

        #region [PublicMethods]

        public void AddDataSet(int currentLevel, int frames, int characterHealth, int combinedEnemyHealth) {
            f_data.Add((currentLevel, frames, characterHealth, combinedEnemyHealth));
        }

        #endregion

        #region [Printing]

        public override string ToString() {
            string ret = f_examinedVariable + ":" + Environment.NewLine;

            for (int i = 0; i < f_data.Count; ++i) {
                ret += f_data[i].currentLevel + "\t" + f_data[i].frames + "\t" + f_data[i].characterHealth + "\t" + f_data[i].combinedEnemyHealth + Environment.NewLine;
            }

            return ret;
        }

        #endregion

    }

    #endregion

    #region [FinalVariables]

    protected SceneLoader.ExaminedVariable f_examinedVariable;
    protected string f_adjective;
    protected string f_noun;
    private ExperimentData f_experimentData;

    #endregion

    #region [PrivateVariables]

    private int m_endCounter;
    private int m_remembered;

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
            m_remembered = m_currentLevel;
            m_currentLevel = 0;
            AdjustValue();
            Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
        }
    }
    public bool Started { get; set; }
    public bool Finished {
        get {
            return Realised && m_endCounter == 2 * TEST_AMOUNT + 1;
        }
    }

    public abstract (string[] adjectives, string[] nouns) FeedbackValues { get; }

    public int CurrentLevel { get { return m_currentLevel; } }

    public string ExperimentText { get { return f_experimentData + ""; } }

    #endregion

    #region [Constructors]

    public Experiment(SceneLoader.ExaminedVariable examinedVariable) {
        f_examinedVariable = examinedVariable;
        f_experimentData = new ExperimentData(f_examinedVariable);
    }

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
#if CHANGING
            if (m_endCounter % 2 == 0) {
                m_currentLevel = 0;
            } else {
                m_currentLevel = m_remembered;
            }
#else
            if (m_endCounter == TEST_AMOUNT) {
                m_currentLevel = 0;
            }
#endif


            ++m_endCounter;
        }

        AdjustValue();
        Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
    }

    public void AddData(int frames, int characterHealth, int combinedEnemyHealth) {
        f_experimentData.AddDataSet(m_currentLevel, frames, characterHealth, combinedEnemyHealth);
    }

    #endregion

    #region [PrivateMethods]

    protected abstract void AdjustValue();

    #endregion

}