using System;

namespace WolfBT {

    public class BTStateReturnFunc : BTState {

        #region [FinalVariables]

        private Func<BTStateReturn> f_func;

        #endregion

        #region [Constructors]

        public BTStateReturnFunc(Func<BTStateReturn> func) {
            f_func = func;
        }

        #endregion

        public override void Enter() {}

        public override BTStateReturn FixedUpdate(int frames) {
            BTStateReturn ret = f_func();
            return ret;           
        }
    }

}