namespace WolfBT {

    /// <summary>
    /// This Node runs several states in sequence and returns either if the first node returns true or false
    /// If none of the nodes returns the specified state; the node returns the result of the last node
    /// </summary>
    public class Sequence : BTState {

        public enum SequenceAbort {
            AbortWhenFalse,
            AbortWhenTrue
        }

        #region [FinalVariables]

        private BTState[] f_states;

        private SequenceAbort f_sequenceAbort;

        #endregion

        #region [PrivateVariables]

        private int m_index;

        #endregion

        #region [Constructors]

        public Sequence(params BTState[] states) {
            f_states = states;
            f_sequenceAbort = SequenceAbort.AbortWhenFalse;
        }

        public Sequence(SequenceAbort sequenceAbort, params BTState[] states) {
            f_states = states;
            f_sequenceAbort = sequenceAbort;
        }

        #endregion

        #region [Override]

        public override void Enter() {
            m_index = 0;
            f_states[m_index].Enter();
        }

        public override BTStateReturn FixedUpdate(int frames) {

            while (true) {
                BTStateReturn ret = f_states[m_index].FixedUpdate(frames);

                switch (ret) {
                    case BTStateReturn.Running:
                    case BTStateReturn.Error:
                        return ret;
                    case BTStateReturn.True:
                        ++m_index;
                        if (f_sequenceAbort == SequenceAbort.AbortWhenTrue || m_index == f_states.Length) {
                            return BTStateReturn.True;
                        }
                        break;
                    case BTStateReturn.False:
                        ++m_index;
                        if (f_sequenceAbort == SequenceAbort.AbortWhenFalse || m_index == f_states.Length) {
                            return BTStateReturn.False;
                        }
                        break;
                }

                f_states[m_index].Enter();
            }

        }

        #endregion

    }

}