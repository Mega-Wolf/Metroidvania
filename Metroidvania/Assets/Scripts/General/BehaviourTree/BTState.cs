namespace WolfBT {

    public abstract class BTState {

        #region [PublicMethods]

        public abstract void Enter();

        public abstract BTStateReturn FixedUpdate(int frames);

        #endregion

    }

}