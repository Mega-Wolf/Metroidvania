using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment = System.Environment;

/// <summary>
/// This class exists throughout all the scenes; loads them and sets the values
/// </summary>
public class SceneLoader : Singleton<SceneLoader> {

    public string finalText = "";

    #region [Types]

    public enum ExaminedVariable {
        BreakTime,
        None,
        AttackSpeed,
        Health,
        Accuracy
    }

    public enum BossFight {
        Owl,
        Rhino,
        Frog
    }

    #endregion

    #region [MemberFields]

    [SerializeField] private GameObject f_permutationCanvas;
    [SerializeField] private GameObject f_canvasRetest;
    [SerializeField] private GameObject f_canvasQuestions;

    [SerializeField] private GameObject f_nextLevelNote;

    [SerializeField] private GameObject f_finalQuestion;

    [SerializeField] private TMPro.TMP_Text f_tryText;

    //[SerializeField] private BossFight m_bossFight = BossFight.Owl;
    //TESTING
    /*[SerializeField]*/
    private BossFight m_bossFight = BossFight.Owl;

    //[SerializeField] private FeedbackAsker f_feedback;

    #endregion

    #region [FinalVariables]

    private Experiment f_owlExperiment;
    private Experiment f_rhinoExperiment;
    private Experiment f_frogExperiment;

    private Dictionary<BossFight, Experiment> f_experiments;

    #endregion

    #region [PrivateVariables]

    private string m_sceneName;
    private bool m_isQuitting = false;

    private ExaminedVariable m_examinedVariable;

    #endregion

    #region [Properties]

    public Experiment CurrentExperiment { get { return f_experiments[m_bossFight]; } }

    public BossFight BossFightValue { get { return m_bossFight; } }

    #endregion

    #region [Init]

    private void Start() {
        f_experiments = new Dictionary<BossFight, Experiment>();
    }

    #endregion

    #region [PublicMethods]

    public void ChooseSetting(int setting) {
        Destroy(f_permutationCanvas);
        if (setting == -1) {
            setting = (int)(UnityEngine.Random.value * Enum.GetValues(typeof(ExaminedVariable)).Length);
        }

        m_examinedVariable = (ExaminedVariable)setting;

        f_owlExperiment = new OwlExperiment((ExaminedVariable)setting);
        f_rhinoExperiment = new RhinoExperiment((ExaminedVariable)setting);
        f_frogExperiment = new FrogExperiment((ExaminedVariable)setting);

        f_experiments[BossFight.Owl] = f_owlExperiment;
        f_experiments[BossFight.Rhino] = f_rhinoExperiment;
        f_experiments[BossFight.Frog] = f_frogExperiment;

        StartScene(false);
    }

    public void StartScene(bool next) {
        f_nextLevelNote.SetActive(false);
        // for now, this always has the same order

        string sceneName;

        // This looks a bit stupid since essentially it is just +=1; however I made this so I could change the order more easily
        if (next) {
            switch (m_bossFight) {
                case BossFight.Owl:
                    m_bossFight = BossFight.Rhino;
                    break;
                case BossFight.Rhino:
                    m_bossFight = BossFight.Frog;
                    break;
                case BossFight.Frog:
                    m_bossFight = (BossFight)(-1);
                    break;
            }
        }

        switch (m_bossFight) {
            case BossFight.Owl:
                sceneName = "OwlScene";
                break;
            case BossFight.Rhino:
                sceneName = "RhinoScene";
                break;
            case BossFight.Frog:
                sceneName = "FrogScene";
                break;
            default:
                //Application.Quit();
                f_finalQuestion.SetActive(true);
                return;
        }

        m_sceneName = sceneName;

        //CurrentExperiment.NextTry();
        //this would not allow me to redo the easiest one; therefore done somewhere else

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        f_tryText.text = "Phase: " + ((CurrentExperiment.EndCounter + 3 - 1) / 3) + "/2   -   Try: " + (1 + (CurrentExperiment.EndCounter - 1) % 3) + "/3";
    }

    public void EndedScene(int frames, int characterHealth, int enemyHealthCombined, int restEnemies) {
        f_tryText.text = "";

        CurrentExperiment.AddData(
            frames, characterHealth, enemyHealthCombined, restEnemies,
            Consts.Instance.Player.PlayerHittingSide.m_hitAmount, Consts.Instance.Player.PlayerHittingSide.m_hittedAmount,
            Consts.Instance.Player.PlayerHittingUp.m_hitAmount, Consts.Instance.Player.PlayerHittingUp.m_hittedAmount,
            Consts.Instance.Player.PlayerHittingDown.m_hitAmount, Consts.Instance.Player.PlayerHittingDown.m_hittedAmount,
            Consts.Instance.Player.m_playerMovement, Consts.Instance.Player.PlayerAir.m_jumpAmount, Consts.Instance.Player.PlayerGrounded.m_dashAmount
            );

        if (m_isQuitting) {
            return;
        }

        Scene currentScene = SceneManager.GetSceneByName(m_sceneName);

        if (currentScene != null) {
            AsyncOperation ao = SceneManager.UnloadSceneAsync(currentScene);
            ao.completed += (AsyncOperation a) => {
                if (m_isQuitting) {
                    return;
                }
                AfterUnload();
            };
            return;
        } else {
            AfterUnload();
        }

    }

    #endregion

    #region [PrivateMethods]

    private void AfterUnload() {
        //TODO; this basically is the only stuff I have to change in this class

        //TODO; is this still allowed ?!?
        if (!CurrentExperiment.Started) {
            f_canvasRetest.SetActive(true);
            return;
        }

        CurrentExperiment.NextTry();

        if (CurrentExperiment.EndCounter == Experiment.TEST_AMOUNT + 1) {
            f_nextLevelNote.SetActive(true);
            f_nextLevelNote.GetComponent<RadioFeedback>().ShowAfterStage1();
            return;
        }


        if (CurrentExperiment.Finished) {
            f_nextLevelNote.SetActive(true);
            f_nextLevelNote.GetComponent<RadioFeedback>().ShowAfterStage2();
            return;
        }

        StartScene(false);
    }

    #endregion

    private void OnApplicationQuit() {
        m_isQuitting = true;

        Debug.Log(
            m_examinedVariable + ":" + Environment.NewLine +
            BossFight.Owl + ":" + Environment.NewLine +
            f_owlExperiment.ExperimentText +
            Environment.NewLine +
            BossFight.Rhino + ":" + Environment.NewLine +
            f_rhinoExperiment.ExperimentText +
            Environment.NewLine +
            BossFight.Frog + ":" + Environment.NewLine +
            f_frogExperiment.ExperimentText +
            Environment.NewLine +
            finalText
        );


        if (!Directory.Exists("Data")) {
            Directory.CreateDirectory("Data");
        }

        for (int i = 0; ; ++i) {
            string path = "Data/Data" + i + ".txt";

            if (!File.Exists(path)) {
                StreamWriter fs = new StreamWriter(File.Create(path));
                fs.Write(
                    m_examinedVariable + ":" + Environment.NewLine +
                    BossFight.Owl + ":" + Environment.NewLine +
                    f_owlExperiment.ExperimentText +
                    Environment.NewLine +
                    BossFight.Rhino + ":" + Environment.NewLine +
                    f_rhinoExperiment.ExperimentText +
                    Environment.NewLine +
                    BossFight.Frog + ":" + Environment.NewLine +
                    f_frogExperiment.ExperimentText +
                    Environment.NewLine +
                    finalText
                );
                fs.Flush();
                fs.Close();
                return;
            }
        }
    }

}