using UnityEngine;

namespace Tools.Menu {

    // public abstract class MenuBuilder : ScriptableObject {

    //     #region [MemberFields]

    //     public GameObject VisualMenuItem;

    //     #endregion

    //     #region [PublicMethods]

    //     public abstract Menu CreateMenu();

    //     #endregion

    // }

    public abstract class Menu : MonoBehaviour {

        #region [MemberFields]

        [SerializeField]
        protected VisualMenuItem f_visualMenuItem;

        #endregion

        #region [PrivateVariables]

        protected MenuItem m_activeMenuItem;

        protected InputHandler m_activeInputHandler;

        #endregion

        #region [Properties]

        public InputHandler ActiveInputHandler { get { return m_activeInputHandler; } }

        #endregion

        #region [PublicMethods]

        public abstract void Show(MenuItem menuItem);

        public abstract void Init();

        #endregion
    }

}