using System;
using UnityEngine;

namespace Tools.Menu {

    // TODO; the Evaluation functions are quite dull at the moment

    #region [Enums]

    // TODO; this indeed does not cover all input keys
    // T** However, it specifies uniquely whether a button is Nort, South, etc. (this will be done as well in the new input system)

    public enum ButtonInteraction {
        KeyA,
        KeyB,
        KeyC,
        KeyD,
        KeyE,
        KeyF,
        KeyG,
        KeyH,
        KeyI,
        KeyJ,
        KeyK,
        KeyL,
        KeyM,
        KeyN,
        KeyO,
        KeyP,
        KeyQ,
        KeyR,
        KeyS,
        KeyT,
        KeyU,
        KeyV,
        KeyW,
        KeyX,
        KeyY,
        KeyZ,
        Key1,
        Key2,
        Key3,
        Key4,
        Key5,
        Key6,
        Key7,
        Key8,
        Key9,
        Key0,
        KeyF1,
        KeyF2,
        KeyF3,
        KeyF4,
        KeyF5,
        KeyF6,
        KeyF7,
        KeyF8,
        KeyF9,
        KeyF10,
        KeyF11,
        KeyF12,
        KeyF13,
        KeyF14,
        KeyF15,
        KeyCtrl,
        KeyShift,
        KeySpace,
        KeyReturn,
        KeyBackspace,
        KeyTab,
        KeyAlt,
        KeyPlus,
        KeyMinus,
        KeyPageUp,
        KeyPageDown,
        //TODO; add more keys (special characters)
        GamepadNorth,
        GamepadWest,
        GampadEast,
        GamepadSouth,
        GamepadStickLeft,
        GamepadStickRight,
        GamepadShoulderLeftFront,
        GamepadShoulderLeftBack,
        GamepadShoulderRightFront,
        GamepadShoulderRightBack,
        GamepadControllLeft,
        GamepadControllRight,
        GamepadDPadUp,
        GamepadDPadLeft,
        GamepadDPadRight,
        GamepadDPadDown,
    }

    public enum AxisInteraction {
        //MouseHorizontal,
        //MouseVertical,
        MouseScrollWheel,
        //GamepadLeftVertical,
        //GamepadLeftHorizontal,
        //GamepadRightVertical,
        //GamepadRightHorizontal,
        //GamepadDPadVertical,
        //GamepadDPadHorizontal,
    }

    public enum VectorInteraction {
        MouseClickLeft,
        MouseClickMiddle,
        MouseClickRight,
        MouseDragLeft, //delta or end?
        MouseDragMiddle, //delta or end?
        MouseDragRight, //delta or end?
        MouseMove,
        GamepadLeft,
        GamepadRight,
        GamepadDPad,
    }

    #endregion

    [Serializable]
    public class EmptyInputCombination {
        public KeyCode buttonPressed;

        //public ButtonInteraction[] necessaryButtons;
        //public ButtonInteraction[] blockingButtons;

        public bool Evaluate(KeyCode interaction) {
            return buttonPressed == interaction;
        }
    }

    [Serializable]
    public class AxisInputCombination {
        public AxisInteraction axisInput;
        //public ButtonInteraction[] necessaryButtons;
        //public ButtonInteraction[] blockingButtons;

        public bool Evaluate(AxisInteraction interaction) {
            return axisInput == interaction;
        }
    }

    [Serializable]
    public class VectorInputCombination {
        public VectorInteraction vectorInput;
        //public ButtonInteraction[] necessaryButtons;
        //public ButtonInteraction[] blockingButtons;

        public bool Evaluate(VectorInteraction interaction) {
            return vectorInput == interaction;
        }
    }

    public class EmptyInputCallback {
        private EmptyInputCombination[] f_inputIndexUp;

        public EmptyInputCombination[] EmptyInputCombinations { get; set; }
        public Func<bool> Callback { get; set; }

        public EmptyInputCallback(EmptyInputCombination[] emptyInputCombinations, Func<bool> callback) {
            EmptyInputCombinations = emptyInputCombinations;
            Callback = callback;
        }

    }

    public class AxisInputCallback {

        public AxisInputCombination[] AxisInputCombinations { get; set; }
        public Func<float, bool> Callback { get; set; }

        public AxisInputCallback(AxisInputCombination[] axisInputCombinations, Func<float, bool> callback) {
            AxisInputCombinations = axisInputCombinations;
            Callback = callback;
        }

    }

    public class VectorInputCallback {

        public VectorInputCombination[] VectorInputCombinations { get; set; }
        public Func<Vector2, bool> Callback { get; set; }

        public VectorInputCallback(VectorInputCombination[] vectorInputCombinations, Func<Vector2, bool> callback) {
            VectorInputCombinations = vectorInputCombinations;
            Callback = callback;
        }

    }

}