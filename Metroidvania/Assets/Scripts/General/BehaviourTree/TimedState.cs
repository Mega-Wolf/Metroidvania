namespace WolfBT {

    public abstract class TimedState {

        #region [PublicMethods]

        public abstract void Enter();

        public abstract BTStateReturn UpdateToPercentage(float percentage);

        #endregion

    }

}