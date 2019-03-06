using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.Menu {

    public class MenuHandler : MonoBehaviour {

        #region [Static]

        private static MenuHandler s_instance;

        public static MenuHandler Instance { get { return s_instance; } }

        static MenuHandler() {
            SceneView.onSceneGUIDelegate += MenuFunc;
        }

        private static void MenuFunc(SceneView sceneView) {
            if (s_instance && s_instance.f_menuStack.Count > 0) {
                s_instance.ReactOnEvent();
            }
        }

        #endregion

        #region [FinalVariables]

        private List<List<Menu>> f_menuStack = new List<List<Menu>>();

        #endregion

        #region [MemberFields]

        [SerializeField]
        private Menu f_dummyStartMenu;

        #endregion

        #region [Init]

        private void OnValidate() {
            s_instance = this;

            f_menuStack.Clear();

            if (f_dummyStartMenu == null) {
                return;
            }

            List<Menu> newStack = new List<Menu>();

            newStack.Add(f_dummyStartMenu);
            f_menuStack.Add(newStack);
        }

        #endregion

        #region [PublicMethods]

        //TODO; I have to refine those methods

        public bool CloseLastOfMenu(Type type) {
            for (int i = f_menuStack.Count; --i >= 0;) {
                for (int j = f_menuStack[i].Count; --j >= 0;) {
                    if (f_menuStack[i][j].GetType() == type) {
                        for (int closeStack = f_menuStack.Count; --closeStack >= i + 1;) {
                            CloseMenuStack();
                        }
                        for (int closeMenu = f_menuStack[i].Count; --closeMenu >= j;) {
                            CloseLastMenu();
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public void RegisterMenu(Menu menu, bool newStack) {

            Debug.Log("RegisterMenu: " + menu.GetType());

            List<Menu> stack;

            Transform lastChild;
            if (newStack) {
                stack = new List<Menu>();
                f_menuStack.Add(stack);

                GameObject go = new GameObject("Stack " + (f_menuStack.Count - 1), typeof(RectTransform));
                lastChild = go.transform;

                go.transform.SetParent(transform);
            } else {
                stack = f_menuStack[f_menuStack.Count - 1];

                lastChild = transform.GetChild(transform.childCount - 1);
            }

            menu.transform.SetParent(lastChild);

            stack.Add(menu);
        }

        public void CloseLastMenu() {
            f_menuStack[f_menuStack.Count - 1].RemoveAt(f_menuStack[f_menuStack.Count - 1].Count - 1);

            Transform lastChild = gameObject.transform.GetChild(gameObject.transform.childCount - 1);
            Transform lastSubChild = lastChild.GetChild(lastChild.childCount - 1);
            DestroyImmediate(lastSubChild.gameObject);

            if (f_menuStack[f_menuStack.Count - 1].Count == 0) {
                CloseMenuStack();
            }
        }

        public void CloseMenuStack() {
            f_menuStack.RemoveAt(f_menuStack.Count - 1);

            Transform lastChild = gameObject.transform.GetChild(gameObject.transform.childCount - 1);
            DestroyImmediate(lastChild.gameObject);
        }

        #endregion

        #region [PrivateMethods]

        private void ReactOnEvent() {

            // TODO
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            //At the moment, I can just do it like that; if the Gamepad is used; I'll have to put a timer to actually poll the data -.-

            switch (Event.current.type) {
                case EventType.KeyDown:
                    HandleEmptyInteraction(Event.current.keyCode);
                    break;
                case EventType.MouseDown:
                    HandleEmptyInteraction(KeyCode.Mouse0 + Event.current.button);
                    break;
                case EventType.MouseLeaveWindow:
                    HandleEmptyInteraction(KeyCode.None);
                    break;

                case EventType.MouseDrag:
                    HandleVectorInteraction(VectorInteraction.MouseDragLeft + Event.current.button, Event.current.delta);
                    break;
                case EventType.MouseMove:
                    HandleVectorInteraction(VectorInteraction.MouseMove, Event.current.mousePosition);
                    break;

                case EventType.ScrollWheel:
                    HandleAxisInteraction(AxisInteraction.MouseScrollWheel, Event.current.delta.y);
                    break;
            }
        }

        //TODO; those three are kind of the same thing; however this is the fastest at the moment

        private void HandleEmptyInteraction(KeyCode interaction) {
            for (int i = f_menuStack.Count; --i >= 0;) {
                for (int j = f_menuStack[i].Count; --j >= 0;) {
                    IEnumerable<EmptyInputCallback> emptyInputCallbacks = f_menuStack[i][j].ActiveInputHandler.EmptyInputCallbacks;
                    foreach (EmptyInputCallback eic in emptyInputCallbacks) {
                        foreach (EmptyInputCombination comb in eic.EmptyInputCombinations) {
                            if (comb.Evaluate(interaction)) {
                                bool consume = eic.Callback();
                                if (consume) {
                                    Event.current.Use();
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HandleAxisInteraction(AxisInteraction interaction, float value) {
            for (int i = f_menuStack.Count; --i >= 0;) {
                for (int j = f_menuStack[i].Count; --j >= 0;) {
                    IEnumerable<AxisInputCallback> axisInputCallbacks = f_menuStack[i][j].ActiveInputHandler.AxisInputCallbacks;
                    foreach (AxisInputCallback aic in axisInputCallbacks) {
                        foreach (AxisInputCombination comb in aic.AxisInputCombinations) {
                            if (comb.Evaluate(interaction)) {
                                bool consume = aic.Callback(value);
                                if (consume) {
                                    Event.current.Use();
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HandleVectorInteraction(VectorInteraction interaction, Vector2 value) {
            for (int i = f_menuStack.Count; --i >= 0;) {
                for (int j = f_menuStack[i].Count; --j >= 0;) {
                    IEnumerable<VectorInputCallback> vectorInputCallbacks = f_menuStack[i][j].ActiveInputHandler.VectorInputCallbacks;
                    foreach (VectorInputCallback vic in vectorInputCallbacks) {
                        foreach (VectorInputCombination comb in vic.VectorInputCombinations) {
                            if (comb.Evaluate(interaction)) {
                                bool consume = vic.Callback(value);
                                if (consume) {
                                    Event.current.Use();
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

    }

}