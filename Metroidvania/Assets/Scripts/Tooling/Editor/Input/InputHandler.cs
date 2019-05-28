using System.Collections.Generic;

namespace Tools.Menu {

    public interface IInputHandler {

        InputHandler ActiveInputHandler { get; }

    }

    public class InputHandler {

        public IEnumerable<EmptyInputCallback> EmptyInputCallbacks { get; set; }
        public IEnumerable<AxisInputCallback> AxisInputCallbacks { get; set; }
        public IEnumerable<VectorInputCallback> VectorInputCallbacks { get; set; }

        public InputHandler(IEnumerable<EmptyInputCallback> emptyInputCallbacks, IEnumerable<AxisInputCallback> axisInputCallbacks, IEnumerable<VectorInputCallback> vectorInputCallbacks) {
            EmptyInputCallbacks = emptyInputCallbacks;
            AxisInputCallbacks = axisInputCallbacks;
            VectorInputCallbacks = vectorInputCallbacks;
        }

    }

}