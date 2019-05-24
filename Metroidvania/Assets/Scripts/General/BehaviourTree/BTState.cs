namespace BehaviourTree {

    public abstract class BTState {

        public enum BTStateReturn {
            True = 1,
            False = 2,
            Running = 4,
            Error = 8
        }

        public abstract void Enter();

        public abstract BTStateReturn FixedUpdate(int frames);

    }

}