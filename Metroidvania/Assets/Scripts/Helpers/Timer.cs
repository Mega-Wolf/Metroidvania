using System;
using System.Collections.Generic;
using UnityEngine;

//TODO; I should probably make a class out of the input data; so I can actually remove that stuff

/// <summary>
/// This thing is not very performant and also basically does the same then using Coroutines everywhere
/// </summary>
/// <typeparam name="Timer"></typeparam>
public class Timer : Singleton<Timer> {

    #region [FinalVariables]

    private List<(int endFrame, Func<bool> ongoingFuncShallAbort, Action endAction)> f_timedActions = new List<(int, Func<bool>, Action)>();

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        for (int i = f_timedActions.Count; --i >= 0;) {
            if (f_timedActions[i].endFrame == GameManager.Instance.Frame) {
                f_timedActions[i].endAction();
                f_timedActions.RemoveAt(i);
            } else {
                bool abort = f_timedActions[i].ongoingFuncShallAbort();
                if (abort) {
                    f_timedActions.RemoveAt(i);
                }
            }
        }
    }

    #endregion

    #region [PublicMethods]

    public void Register(int frames, Func<bool> ongoingFuncShallAbort, Action endAction) {
        f_timedActions.Add((GameManager.Instance.Frame + frames, ongoingFuncShallAbort, endAction));
    }

    // public void RemoveTimedFunction(Action action) {
    //     for (int i = f_timedActions.Count; --i >= 0;) {
    //         if (f_timedActions[i].endAction == action) {
    //             f_timedActions.RemoveAt(i);
    //         }
    //     }
    // }

    #endregion


}