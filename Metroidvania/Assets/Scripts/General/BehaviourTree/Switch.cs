using static System.Diagnostics.Debug;

namespace WolfBT {

    /// <summary>
    /// Like sequence, but index can be chosen to not get reset when left
    /// </summary>
    public class Switch : BTState {

        #region [FinalVariables]

        private readonly bool f_resetIndex;
        private readonly BTState[] f_states;
        private readonly BTStateReturn f_abortState;

        #endregion

        #region [PrivateVariables]

        private int m_startIndex = 0;
        private int m_index = 0;

        #endregion

        #region [Constructors]

        public Switch(BTState[] states, bool resetIndex, BTStateReturn abortState) {
            Assert(abortState < BTStateReturn.Running);
            Assert(!(abortState == (BTStateReturn.True | BTStateReturn.False) && resetIndex));

            f_resetIndex = resetIndex;
            f_abortState = abortState;
        }

        #endregion

        #region [Override]

        public override void Enter() {
            if (f_resetIndex) {
                m_index = 0;
            }
            m_startIndex = m_index;

            f_states[m_index].Enter();
        }

        public override BTStateReturn FixedUpdate(int frames) {

            while (true) {
                BTStateReturn ret = f_states[m_index].FixedUpdate(frames);

                if (ret == BTStateReturn.Running) {
                    return BTStateReturn.Running;
                }

                m_index = (m_index + 1) % f_states.Length;

                if (ret == BTStateReturn.True && (f_abortState & BTStateReturn.True) != 0) {
                    return BTStateReturn.True;
                }

                if (ret == BTStateReturn.False && (f_abortState & BTStateReturn.False) != 0) {
                    return BTStateReturn.False;
                }

                if (m_index == m_startIndex) {
                    return ret;
                }

                f_states[m_index].Enter();
            }

        }

        #endregion

    }

}