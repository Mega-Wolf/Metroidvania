using System;
using System.Collections.Generic;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Tools.Menu {

    // [CreateAssetMenu(fileName = "RadialMenu", menuName = "Metroidvania/Menus/RadialMenu", order = 1)]
    // public class RadialMenuBuilder : MenuBuilder {

    //     #region [MemberFields]

    //     [Header("General Info")]

    //     public float Radius = 3;

    //     public MenuItemBuilder StartMenuItem;

    //     public GameObject PreVisualMenuItem;

    //     [Header("Inputs")]

    //     public EmptyInputCombination[] InputIndexUp;

    //     public EmptyInputCombination[] InputIndexDown;

    //     public EmptyInputCombination[] InputBack;

    //     public EmptyInputCombination[] InputExecute;

    //     // Gamepad stuff

    //     public VectorInputCombination[] InputDeltaDirection;

    //     [Tooltip("Gets a button-like input, but will read out the cursor. Usefull for interpreting a gamepad button as a mouse click with a position.")]
    //     public EmptyInputCombination[] InputInterpretCursor;

    //     //TODO; F1..F15

    //     #endregion

    //     #region [Override]

    //     public override Menu CreateMenu() {
    //         return new RadialMenu(this);
    //     }

    //     #endregion

    // }

    public class RadialMenu : Menu {

        #region [MemberFields]

        [Header("General Info")]

        [SerializeField]
        private float f_radius = 3;

        [SerializeField]
        private MenuItemBuilder f_startMenuItem;

        [Header("Inputs")]

        [SerializeField]
        private EmptyInputCombination[] f_inputIndexUp;

        [SerializeField]
        private EmptyInputCombination[] f_inputIndexDown;

        [SerializeField]
        private EmptyInputCombination[] f_inputBack;

        [SerializeField]
        private EmptyInputCombination[] f_inputExecute;

        // Gamepad stuff

        [SerializeField]
        private VectorInputCombination[] f_inputDeltaDirection;

        [Tooltip("Gets a button-like input, but will read out the cursor. Usefull for interpreting a gamepad button as a mouse click with a position.")]
        [SerializeField]
        private EmptyInputCombination[] f_inputInterpretCursor;

        //TODO; F1..F15

        #endregion

        #region [FinalVariables]

        private EmptyInputCallback[] f_boolInputCallbacks;
        private AxisInputCallback[] f_axisInputCallbacks;
        private VectorInputCallback[] f_vectorInputCallbacks;

        private List<GameObject> f_elements = new List<GameObject>();

        #endregion

        #region [Init]

        public override void Init() {
            MenuHandler.Instance.RegisterMenu(this, true);

            EmptyInputCallback inputIndexUpCallback = new EmptyInputCallback(f_inputIndexUp, () => ChangeIndex(true));
            EmptyInputCallback inputIndexDownCallback = new EmptyInputCallback(f_inputIndexUp, () => ChangeIndex(false));

            EmptyInputCallback inputInterpretCursorCallback = new EmptyInputCallback(f_inputInterpretCursor, InterpretCursor);

            //EmptyInputCallback inputExecuteCallback = new EmptyInputCallback(f_inputExecute, () => ExecuteMenuItem(m_activeMenuItem.Children[m_focusedIndex]));

            //VectorInputCallback inputPointingCallback = new VectorInputCallback(f_inputPointing, DetermineRadialSelection);

            EmptyInputCallback[] boolInputCallbacks = { inputIndexUpCallback, inputIndexDownCallback, inputInterpretCursorCallback };
            AxisInputCallback[] axisInputCallbacks = { };
            VectorInputCallback[] vectorInputCallbacks = { };

            m_activeInputHandler = new InputHandler(
                boolInputCallbacks,
                axisInputCallbacks,
                vectorInputCallbacks
            );

            // Those two lines are essentially the same, but to stick to the new guide, the Execute is called
            f_startMenuItem.CreateMenuItem(null).Execute(this);
            //Show(f_startMenuItem.CreateMenuItem(null));
        }

        #endregion

        #region [Override]

        public override void Show(MenuItem menuItem) {

            DeleteObjects();

            if (menuItem.Children.IsNullOrEmpty()) {
                return;
            }

            m_activeMenuItem = menuItem;

            for (int i = 0; i < menuItem.Children.Count; ++i) {
                Vector3 add = Quaternion.Euler(0, 0, -(360 / menuItem.Children.Count) * i) * new Vector3(0, f_radius, 0);
                GameObject newGO = Instantiate(f_visualMenuItem.gameObject, /*m_currentClick*/  add, Quaternion.identity, transform);
                newGO.name = "MenuItem" + i;

                VisualMenuItem vmi = newGO.GetComponent<VisualMenuItem>();
                vmi.Draw(m_activeMenuItem.Children[i].ItemData);
                //m_activeMenuItem.Children[i].Show(vmi);


                f_elements.Add(newGO);
            }
            //TODO; back button
            // GameObject newGO2 = Instantiate(pre_visibleMenuItem, m_currentClick, Quaternion.identity, f_canvas.transform);
            // newGO2.name = "MenuItem-1";
            // m_dummies.Add(newGO2);

            //TODO
            //SetMenuIndex(0);
        }

        private void DeleteObjects() {
            foreach (GameObject go in f_elements) {
                DestroyImmediate(go);
            }
        }

        #endregion

        #region [PrivateMethods]

        private bool InterpretCursor() {

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Collider2D[] hits = Physics2D.OverlapCircleAll(mouseRay.origin, 0.1f, LayerMask.GetMask("UI"));
            foreach (Collider2D hit in hits) {
                string name = hit.gameObject.name;
                if (name.StartsWith("MenuItem")) {
                    int number = int.Parse(name.Substring("MenuItem".Length));
                    m_activeMenuItem.Children[number].Execute(this);
                    return true;
                }
            }
            return false;
        }

        private bool ChangeIndex(bool increase) {
            //TODO

            return true;
        }

        // private bool DetermineRadialSelection(Vector2 vector) {
        //     //TODO; this is done for the Gamepad
        //     return true;
        // }

        #endregion

    }

}