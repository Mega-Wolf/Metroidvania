using System.Collections.Generic;
using UnityEngine;

namespace Tools.Menu {

    public class MenuItemList : MenuItem {

        #region [Constructors]

        public MenuItemList(MenuItem parent, MenuItemListBuilder menuItemListBuilder) : base(parent) {
            Children = new List<MenuItem>();
            foreach (MenuItemBuilder mib in menuItemListBuilder.MenuItems) {
                Children.Add(mib.CreateMenuItem(this));
            }
        }

        #endregion

        #region [Override]

        public override void Execute(Menu menu) {
            menu.Show(this);
        }

        #endregion

    }

}