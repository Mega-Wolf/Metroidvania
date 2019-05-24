namespace BehaviourTree {

    /// <summary>
    /// This state maps the return of a true or false state to a specified value
    /// This way, you can realise a negater, but also always map to true or false
    /// </summary>
    public class Mapper : BTState {

        #region [FinalVariables]

        private readonly BTState f_childState;
        private readonly BTStateReturn f_trueValue;
        private readonly BTStateReturn f_falseValue;

        #endregion

        #region [Constructors]

        public Mapper(BTState child, bool trueValue, bool falseValue) {
            f_childState = child;

            if (trueValue) {
                f_trueValue = BTStateReturn.True;
            } else {
                f_trueValue = BTStateReturn.False;
            }

            if (falseValue) {
                f_falseValue = BTStateReturn.True;
            } else {
                f_falseValue = BTStateReturn.False;
            }
        }

        #endregion

        #region [Override]

        public override void Enter() {
            f_childState.Enter();
        }

        public override BTStateReturn FixedUpdate(int frames) {
            BTStateReturn ret = f_childState.FixedUpdate(frames);

            switch (ret) {
                case BTStateReturn.Running:
                    return BTStateReturn.Running;
                case BTStateReturn.True:
                    return f_trueValue;
                case BTStateReturn.False:
                    return f_falseValue;
                default:
                    return BTStateReturn.Error;
            }
        }

        #endregion
    }

}