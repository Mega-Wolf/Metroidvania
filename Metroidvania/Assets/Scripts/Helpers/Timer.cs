using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This thing is not very performant and also basically does the same then using Coroutines everywhere
/// </summary>
/// <typeparam name="Timer"></typeparam>
public class Timer : Singleton<Timer> {

    #region [FinalVariables]

    private List<(int endFrame, Action action)> f_timedActions = new List<(int, Action)>();

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        for (int i = f_timedActions.Count; --i >= 0;) {
            if (f_timedActions[i].endFrame == GameManager.Instance.Frame) {
                f_timedActions[i].action();
                f_timedActions.RemoveAt(i);
            }
        }
    }

    #endregion

    #region [PublicMethods]

    public void Register(int frames, Action action) {
        f_timedActions.Add((GameManager.Instance.Frame + frames, action));
    }

    public void StopTimer(Action action) {
        for (int i = f_timedActions.Count; --i >= 0;) {
            if (f_timedActions[i].action == action) {
                f_timedActions.RemoveAt(i);
            }
        }
    }

    #endregion


}