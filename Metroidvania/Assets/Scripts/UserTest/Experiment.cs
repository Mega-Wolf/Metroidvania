// #define CHANGING

// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public abstract class Experiment {

//     // TESTING
//     private const int TEST_AMOUNT = 5;

//     #region [Types]

//     public class ExperimentData {

//         #region [FinalVariables]

//         private SceneLoader.ExaminedVariable f_examinedVariable;
//         private List<(int currentLevel, int frames, int characterHealth, int combinedEnemyHealth,
//         int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits,
//         Vector2 movement, int jumps, int dashes)> f_data = new List<(int, int, int, int,
//         int, int, int, int, int, int,
//         Vector2, int, int)>();

//         #endregion

//         #region [Constructors]

//         public ExperimentData(SceneLoader.ExaminedVariable examinedVariable) {
//             f_examinedVariable = examinedVariable;
//         }

//         #endregion

//         #region [PublicMethods]

//         public void AddDataSet(int currentLevel, int frames, int characterHealth, int combinedEnemyHealth, int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits,
//         Vector2 movement, int jumps, int dashes) {
//             f_data.Add((currentLevel, frames, characterHealth, combinedEnemyHealth, sideTries, sideHits, upTries, upHits, downTries, downHits,
//         movement, jumps, dashes));
//         }

//         #endregion

//         #region [Printing]

//         public override string ToString() {
//             string ret = f_examinedVariable + ":" + Environment.NewLine;

//             for (int i = 0; i < f_data.Count; ++i) {
//                 ret += f_data[i].currentLevel + "\t" +
//                 f_data[i].frames + "\t" + f_data[i].characterHealth + "\t" + f_data[i].combinedEnemyHealth + "\t" +
//                 f_data[i].sideTries + "\t" + f_data[i].sideHits + "\t" + f_data[i].upTries + "\t" + f_data[i].upHits + "\t" + f_data[i].downTries + "\t" + f_data[i].downHits + "\t" +
//                 f_data[i].movement.x + "\t" + f_data[i].movement.y + "\t" + f_data[i].jumps + "\t" + f_data[i].dashes + Environment.NewLine;
//                 ;
//             }

//             return ret;
//         }

//         #endregion

//     }

//     #endregion

//     #region [FinalVariables]

//     protected SceneLoader.ExaminedVariable f_examinedVariable;
//     protected string f_text;
//     private ExperimentData f_experimentData;

//     #endregion

//     #region [PrivateVariables]

//     private int m_endCounter;
//     private int m_remembered;

//     #endregion

//     #region [PrivateProperties]

//     protected int m_currentLevel { get; private set; }

//     #endregion

//     #region [Properties]

//     public string Text { get { return f_text; } }

//     private bool _realised = false;
//     public bool Realised {
//         get {
//             return _realised;
//         }
//         set {
//             _realised = value;
//             m_currentLevel -= 2;
//             m_endCounter = 1;
//             m_remembered = m_currentLevel;
//             m_currentLevel = 0;
//             AdjustValue();
//             Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
//         }
//     }
//     public bool Started { get; set; }
//     public bool Finished {
//         get {
//             return Realised && m_endCounter == 2 * TEST_AMOUNT + 1;
//         }
//     }

//     public abstract string[] FeedbackTexts { get; }

//     public int CurrentLevel { get { return m_currentLevel; } }

//     public int EndCounter { get { return m_endCounter; } }

//     public string ExperimentText { get { return f_experimentData + ""; } }

//     #endregion

//     #region [Constructors]

//     public Experiment(SceneLoader.ExaminedVariable examinedVariable) {
//         f_examinedVariable = examinedVariable;
//         f_experimentData = new ExperimentData(f_examinedVariable);
//     }

//     #endregion

//     #region [PublicMethods]

//     public void NextTry() {
//         if (!Started) {
//             return;
//         }
//         // if (Realised) {
//         //     --m_currentLevel;
//         // } else {
//         //     ++m_currentLevel;
//         // }

//         if (!Realised) {
//             ++m_currentLevel;
//         } else {
// #if CHANGING
//             if (m_endCounter % 2 == 0) {
//                 m_currentLevel = 0;
//             } else {
//                 m_currentLevel = m_remembered;
//             }
// #else
//             if (m_endCounter == TEST_AMOUNT) {
//                 m_currentLevel = 0;
//             }
// #endif


//             ++m_endCounter;
//         }

//         AdjustValue();
//         Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
//     }

//     public void AddData(int frames, int characterHealth, int combinedEnemyHealth, int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits, Vector2 movement, int jumps, int dashes) {
//         f_experimentData.AddDataSet(m_currentLevel, frames, characterHealth, combinedEnemyHealth, sideTries, sideHits, upTries, upHits, downTries, downHits, movement, jumps, dashes);
//     }

//     #endregion

//     #region [PrivateMethods]

//     protected abstract void AdjustValue();

//     #endregion

// }




#define CHANGING

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Experiment {

    // TESTING (I want 5 to throw away the first 2)
    private const int TEST_AMOUNT = 3;

    #region [Types]

    public class ExperimentData {

        #region [FinalVariables]

        private SceneLoader.ExaminedVariable f_examinedVariable;
        private List<(float currentLevel, int frames, int characterHealth, int combinedEnemyHealth, int restEnemies,
        int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits,
        Vector2 movement, int jumps, int dashes)> f_data = new List<(float, int, int, int, int,
        int, int, int, int, int, int,
        Vector2, int, int)>();

        #endregion

        #region [Constructors]

        public ExperimentData(SceneLoader.ExaminedVariable examinedVariable) {
            f_examinedVariable = examinedVariable;
        }

        #endregion

        #region [PublicMethods]

        public void AddDataSet(float currentLevel, int frames, int characterHealth, int combinedEnemyHealth, int restEnemies, int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits, Vector2 movement, int jumps, int dashes) {
            f_data.Add((currentLevel, frames, characterHealth, combinedEnemyHealth, restEnemies, sideTries, sideHits, upTries, upHits, downTries, downHits, movement, jumps, dashes));
        }

        #endregion

        #region [Printing]

        public override string ToString() {
            string ret = f_examinedVariable + ":" + Environment.NewLine;

            for (int i = 0; i < f_data.Count; ++i) {
                ret += f_data[i].currentLevel + "\t" +
                f_data[i].frames + "\t" + f_data[i].characterHealth + "\t" + f_data[i].combinedEnemyHealth + "\t" +
                f_data[i].sideTries + "\t" + f_data[i].sideHits + "\t" + f_data[i].upTries + "\t" + f_data[i].upHits + "\t" + f_data[i].downTries + "\t" + f_data[i].downHits + "\t" +
                f_data[i].movement.x + "\t" + f_data[i].movement.y + "\t" + f_data[i].jumps + "\t" + f_data[i].dashes + Environment.NewLine;
                ;
            }

            return ret;
        }

        #endregion

    }

    #endregion

    #region [FinalVariables]

    protected SceneLoader.ExaminedVariable f_examinedVariable;
    private ExperimentData f_experimentData;

    #endregion

    #region [PrivateVariables]

    private int m_endCounter;

    private bool m_isEasier = false;

    #endregion

    #region [PrivateProperties]

    protected float m_currentLevel { get; private set; }

    #endregion

    #region [Properties]

    public string ExperimentText { get { return f_experimentData + ""; } }

    public bool Started { get; set; }
    public bool Finished {
        get {
            return m_endCounter == 2 * TEST_AMOUNT + 1;
        }
    }

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

        if (m_isEasier) {
            m_currentLevel = 2.5f;
        } else {
            m_currentLevel = 0;
        }

        ++m_endCounter;

        if (m_endCounter == TEST_AMOUNT) {
            m_isEasier = true;
        }

        AdjustValue();
        Debug.LogWarning("Future will test with m_currentLevel = " + m_currentLevel);
    }

    public void AddData(int frames, int characterHealth, int combinedEnemyHealth, int restEnemies, int sideTries, int sideHits, int upTries, int upHits, int downTries, int downHits, Vector2 movement, int jumps, int dashes) {
        f_experimentData.AddDataSet(m_currentLevel, frames, characterHealth, combinedEnemyHealth, restEnemies, sideTries, sideHits, upTries, upHits, downTries, downHits, movement, jumps, dashes);
    }

    #endregion

    #region [PrivateMethods]

    protected abstract void AdjustValue();

    #endregion

}