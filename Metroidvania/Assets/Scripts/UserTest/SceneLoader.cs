using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class exists throughout all the scenes; loads them and sets the values
/// </summary>
public class SceneLoader : Singleton<SceneLoader> {

    #region [Types]

    public enum ExaminedVariable {
        CastTime,
        AttackSpeed,
        Health
    }

    private enum BossFight {
        Owl,
        Rhino,
        Frog
    }

    #endregion

    #region [MemberFields]

    [SerializeField] private GameObject f_permutationCanvas;
    [SerializeField] private GameObject f_canvasRetest;
    [SerializeField] private GameObject f_canvasQuestions;

    [SerializeField] private BossFight m_bossFight = BossFight.Owl;

    [SerializeField] private FeedbackAsker f_feedback;

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

    #endregion

    #region [Properties]

    public Experiment CurrentExperiment { get { return f_experiments[m_bossFight]; } }

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
            setting = (int)(Random.value * 6);
        }

        //   :      Owl     Rhino       Frog
        // --+------------------------------
        //  0:      A       C           H
        //  1:      A       H           C
        //  2:      C       A           H
        //  3:      C       H           A
        //  4:      H       A           C
        //  5:      H       C           A

        switch (setting) {
            case 0:
            case 1:
                f_owlExperiment = new OwlExperiment(ExaminedVariable.AttackSpeed);
                break;
            case 2:
            case 3:
                f_owlExperiment = new OwlExperiment(ExaminedVariable.CastTime);
                break;
            case 4:
            case 5:
                f_owlExperiment = new OwlExperiment(ExaminedVariable.Health);
                break;
        }

        switch (setting) {
            case 2:
            case 4:
                f_rhinoExperiment = new RhinoExperiment(ExaminedVariable.AttackSpeed);
                break;
            case 0:
            case 5:
                f_rhinoExperiment = new RhinoExperiment(ExaminedVariable.CastTime);
                break;
            case 1:
            case 3:
                f_rhinoExperiment = new RhinoExperiment(ExaminedVariable.Health);
                break;
        }

        switch (setting) {
            case 3:
            case 5:
                f_frogExperiment = new FrogExperiment(ExaminedVariable.AttackSpeed);
                break;
            case 1:
            case 4:
                f_frogExperiment = new FrogExperiment(ExaminedVariable.CastTime);
                break;
            case 0:
            case 2:
                f_frogExperiment = new FrogExperiment(ExaminedVariable.Health);
                break;
        }

        f_experiments[BossFight.Owl] = f_owlExperiment;
        f_experiments[BossFight.Rhino] = f_rhinoExperiment;
        f_experiments[BossFight.Frog] = f_frogExperiment;

        StartScene(false);
    }

    public void StartScene(bool next) {
        // for now, this always has the same order

        string sceneName;

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
                //TODO; not sure if I shall just quit
                // actual TODO: save the data somewhere
                Application.Quit();
                return;
        }

        m_sceneName = sceneName;

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

        //CurrentExperiment.NextTry();
        //TODO; this will not allow me to redo the easiest one; therefore done somewhere else

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void EndedScene() {
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
                if (!CurrentExperiment.Started) {
                    f_canvasRetest.SetActive(true);
                    return;
                }

                CurrentExperiment.NextTry();
                if (!CurrentExperiment.Realised) {
                    f_feedback.ShowQuestions(CurrentExperiment);
                    return;
                }

                StartScene(CurrentExperiment.Finished);
            };
            return;
        }

        if (!CurrentExperiment.Started) {
            f_canvasRetest.SetActive(true);
            return;
        }

        if (!CurrentExperiment.Realised) {
            f_canvasQuestions.SetActive(true);
            return;
        }

        StartScene(CurrentExperiment.Finished);
    }

    #endregion

    private void OnApplicationQuit() {
        m_isQuitting = true;
    }

}