using UnityEngine;

namespace WolfBT {

    public class TimerState : BTState {

        #region [Operators]

        public static implicit operator TimerState((TimedState timedState, int frames) data) {
            return new TimerState(data.timedState, data.frames);
        }

        #endregion

        #region [FinalVariables]

        private TimedState f_timedState;
        private int f_frameDuration;

        #endregion

        #region [PrivateVariables]

        private int m_currentFrames;

        #endregion

        #region [Constructors]

        public TimerState(TimedState timedState, int frames) {
            f_timedState = timedState;
            f_frameDuration = frames;
        }

        #endregion

        #region [Override]

        public override void Enter() {
            m_currentFrames = 0;
            f_timedState.Enter();
        }

        public override BTStateReturn FixedUpdate(int frames) {
            m_currentFrames = Mathf.Min(m_currentFrames + frames, f_frameDuration);
            BTStateReturn timedStateRet = f_timedState.UpdateToPercentage(m_currentFrames / (float)f_frameDuration);

            if (timedStateRet != BTStateReturn.Running) {
                return timedStateRet;
            }

            if (m_currentFrames == f_frameDuration) {
                return BTStateReturn.True;
            }

            return BTStateReturn.Running;
        }

        #endregion

    }

}