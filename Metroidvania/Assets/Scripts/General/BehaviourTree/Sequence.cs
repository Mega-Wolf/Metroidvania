namespace BehaviourTree {

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

        public Sequence(BTState[] states, SequenceAbort sequenceAbort) {
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
                        return BTStateReturn.Running;
                    case BTStateReturn.True:
                        ++m_index;
                        return (f_sequenceAbort == SequenceAbort.AbortWhenTrue || m_index == f_states.Length) ? BTStateReturn.True : BTStateReturn.Running;
                    case BTStateReturn.False:
                        ++m_index;
                        return (f_sequenceAbort == SequenceAbort.AbortWhenFalse || m_index == f_states.Length) ? BTStateReturn.False : BTStateReturn.Running;
                }

                f_states[m_index].Enter();

            }

        }

        #endregion

    }

}