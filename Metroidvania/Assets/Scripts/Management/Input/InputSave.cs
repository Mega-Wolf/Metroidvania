using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class saves InputData collected by the game
/// </summary>
[System.Serializable]
public class InputSave {

    #region [Types]

    [System.Serializable]
    public class NestedInputData {

        public List<InputData> List;

        public NestedInputData(List<InputData> list) {
            List = list;
        }

        public static implicit operator List<InputData>(NestedInputData nid) {
            return nid.List;
        }

        public static implicit operator NestedInputData(List<InputData> list) {
            return new NestedInputData(list);
        }

        // public InputData this[int index] {
        //     get {
        //         return List[index];
        //     }
        //     set {
        //         List[index] = value;
        //     }
        // }

    }

    [System.Serializable]
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

    #region [FinalVariables]

    [SerializeField]
    private List<NestedInputData> f_captures = new List<NestedInputData>();

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
        f_captures[f_captures.Count - 1].List.Add(new InputData(button, value));
    }

    public void Split() {
        f_captures.Add(new List<InputData>());
    }

    public List<InputData> GetLayer(int index) {
        return f_captures[index];
    }

    #endregion

}