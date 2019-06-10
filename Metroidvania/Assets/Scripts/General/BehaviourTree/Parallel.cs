using static System.Diagnostics.Debug;

namespace WolfBT {

    /// <summary>
    /// This Node runs several Nodes in parallel and returns the first one which returns a value
    /// </summary>
    public class Parallel : BTState {

        #region [FinalVariables]

        private BTState[] f_states;

        #endregion

        #region [Constructors]

        public Parallel(BTState[] states) {
            Assert(states.Length > 0);
            f_states = states;
        }

        #endregion

        #region [Override]

        public override void Enter() {
            for (int i = 0; i < f_states.Length; ++i) {
                f_states[i].Enter();
            }
        }

        public override BTStateReturn FixedUpdate(int frames) {
            for (int i = 0; i < f_states.Length; ++i) {
                BTStateReturn ret = f_states[i].FixedUpdate(frames);
                if (ret != BTStateReturn.Running) {
                    return ret;
                }
            }

            return BTStateReturn.Running;
        }

        #endregion

    }

}