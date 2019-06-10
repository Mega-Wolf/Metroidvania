namespace WolfBT {

    public class Loop : BTState {

        #region [FinalVariables]

        private BTState f_state;

        #endregion

        #region [Constructors]

        public Loop(BTState state) {
            f_state = state;
        }

        #endregion

        #region [Override]

        public override void Enter() { }

        public override BTStateReturn FixedUpdate(int frames) {
            f_state.FixedUpdate(frames);
            return BTStateReturn.Running;
        }

        #endregion

    }

}