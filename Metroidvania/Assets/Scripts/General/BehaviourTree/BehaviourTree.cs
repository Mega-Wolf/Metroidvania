namespace BehaviourTree {

    public class BehaviourTree {

        #region [FinalVariables]

        private BTState f_root;

        #endregion

        #region [Constructors]

        public BehaviourTree(BTState root) {
            f_root = root;
            f_root.Enter();
        }

        #endregion

        #region [PublicMethods]

        public void FixedUpdate(int frames) {
            f_root.FixedUpdate(frames);
        }

        #endregion

    }

}