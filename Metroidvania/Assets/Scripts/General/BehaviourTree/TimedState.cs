namespace WolfBT {

    public abstract class TimedState {

        #region [PublicMethods]

        public abstract void Enter();

        public abstract BTStateReturn UpdateToPercentage(float percentage);

        #endregion

    }

    public class NOPTimedState : TimedState {

        #region [Override]

        public override void Enter() { }

        public override BTStateReturn UpdateToPercentage(float percentage) {
            return BTStateReturn.Running;
        }

        #endregion
    }

}