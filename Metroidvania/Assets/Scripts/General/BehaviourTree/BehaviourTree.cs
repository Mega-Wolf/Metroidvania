namespace WolfBT {

//TODO; This is the same as any normal Action BTState

    public class BehaviourTree : BTState {

        #region [FinalVariables]

        private BTState f_root;

        #endregion

        #region [Constructors]

        public BehaviourTree(BTState root) {
            f_root = root;
        }

        #endregion

        #region [Override]

        public override void Enter() {
            f_root.Enter();
        }

        public override BTStateReturn FixedUpdate(int frames) {
            BTStateReturn ret = f_root.FixedUpdate(frames);
            if (ret != BTStateReturn.Running) {
                f_root.Enter();
            }
            return ret;
        }

        #endregion

    }

}