using System.Collections.Generic;
/// <summary>
/// This class saves InputData collected by the game
/// </summary>
public class InputSave {

    #region [Types]

    public struct InputData {

        public string Button;
        public bool Value;

        public int Frame;
        //public EMenuKey MenuKey;


        public InputData(string button, bool value) {
            Button = button;
            Value = value;

            Frame = InputManager.Instance.ReplayFrame;
        }

        // public InputData(EMenuKey menuKey) {

        // }

    }

    #endregion

    #region [PrivateVariables]

    private List<List<InputData>> f_captures = new List<List<InputData>>();

    #endregion

    #region [Constructors]

    public InputSave() {
        Split();
    }

    #endregion

    #region [PublicMethods]

    //TODO; since the InputSave at the moment is at the InputManagers; it will need some work to integrate the menu related inputs
    //T** At leat replaying shold probably be in an own class

    public void AddButtonInput(string button, bool value) {
        f_captures[f_captures.Count - 1].Add(new InputData(button, value));
    }

    public void Split() {
        f_captures.Add(new List<InputData>());
    }

    public List<InputData> GetLayer(int index) {
        return f_captures[index];
    }

    #endregion

}