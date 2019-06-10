using System;

namespace WolfBT {

    public class ActionGroup : BTState {

        #region [Operators]

        public static implicit operator ActionGroup(Action[] actions) {
            return new ActionGroup(actions);
        }

        #endregion

        #region [FinalVariables]

        private Action[] f_actions;

        #endregion

        #region [Constructors]

        public ActionGroup(params Action[] actions) {
            f_actions = actions;
        }

        #endregion

        public override void Enter() {}

        public override BTStateReturn FixedUpdate(int frames) {
            // Done here so it can be repeated in a loop

            for (int i = 0; i < f_actions.Length; ++i) {
                f_actions[i]();
            }
            return BTStateReturn.True;
        }
    }

}