using System.Collections.Generic;
using UnityEngine;

namespace Tools.Menu {

    public abstract class MenuItemBuilder : ScriptableObject {

        #region [PublicMethods]

        public abstract MenuItem CreateMenuItem(MenuItem parent);

        #endregion

    }

    public abstract class MenuItem {

        #region [FinalVariables]

        private MenuItem f_parent;

        #endregion

        #region [Constructors]

        public MenuItem(MenuItem parent) {
            f_parent = parent;
        }

        #endregion

        #region [Properties]

        //int ActiveFocus { get; set; }

        public MenuItem Parent { get { return f_parent; } }

        public List<MenuItem> Children { get; protected set; }

        public ItemData ItemData { get; protected set; }

        //Menu Menu { get; }

        #endregion

        #region [PublicMethods]

        public abstract void Execute(Menu menu);

        #endregion

    }

}