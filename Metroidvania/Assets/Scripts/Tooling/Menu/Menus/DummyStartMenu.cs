using System;
using UnityEngine;

namespace Tools.Menu {

    // TODO; this class should also handle moving around with the gamepad cursor
    // T** also: should menus be opened with vector? this would be correct with a click; but a button would have to ask where the cursor is
    // T** how will I handle that -.-
    // T** if necessary; I will just ask for the cursor position I guess

    [Serializable]
    public class OpenMenuData {

        #region [MemberFields]

        [SerializeField]
        private Menu pre_menu;

        [SerializeField]
        private EmptyInputCombination[] f_openCombinations;

        #endregion

        #region [Properties]

        public Menu Menu { get { return pre_menu; } }

        public EmptyInputCombination[] OpenCombinations { get { return f_openCombinations; } }

        #endregion

    }

    // [CreateAssetMenu(fileName = "DummyStartMenu", menuName = "Metroidvania/Menus/DummyStartMenu", order = 0)]
    // public class DummyStartMenuBuilder : MenuBuilder {

    //     #region [MemberFields]

    //     [SerializeField]
    //     private OpenMenuData[] f_openMenuData;

    //     #endregion

    //     #region [Override]

    //     public override Menu CreateMenu() {
    //         return new DummyStartMenu(f_openMenuData);
    //     }

    //     #endregion

    // }

    public class DummyStartMenu : Menu {

        #region [MemberFields]

        [SerializeField]
        private OpenMenuData[] f_openMenuData;

        #endregion

        #region [Constructors]

        private void OnValidate() {

            Debug.Log("Created Dummy");

            EmptyInputCallback[] emptyInputCallbacks = new EmptyInputCallback[f_openMenuData.Length];

            for (int i = 0; i < f_openMenuData.Length; ++i) {
                OpenMenuData omd = f_openMenuData[i];
                emptyInputCallbacks[i] = new EmptyInputCallback(omd.OpenCombinations, () => OpenMenu(omd.Menu));
            }

            m_activeInputHandler = new InputHandler(
                emptyInputCallbacks,
                new AxisInputCallback[0],
                new VectorInputCallback[0]
            );

        }

        #endregion

        #region [Override]

        public override void Show(MenuItem menuItem) {
            // EMPTY, since this is never reall opened
        }

        #endregion

        #region [PrivateMethods]

        private bool OpenMenu(Menu menu) {

            MenuHandler.Instance.CloseLastOfMenu(menu.GetType());

            GameObject newMenuGO = Instantiate(menu.gameObject);
            newMenuGO.name = newMenuGO.gameObject.name;
            Menu newMenu = newMenuGO.GetComponent<Menu>();
            newMenu.Init();

            return true;
        }

        public override void Init() {
            throw new NotImplementedException();
        }

        #endregion
    }

}