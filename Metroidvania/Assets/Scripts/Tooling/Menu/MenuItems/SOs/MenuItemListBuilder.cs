using System.Collections.Generic;
using UnityEngine;

namespace Tools.Menu {

    [CreateAssetMenu(fileName = "MenuItemList", menuName = "Metroidvania/MenuItems/MenuItemList", order = 0)]
    public class MenuItemListBuilder : MenuItemBuilder {

        #region [MemberFields]

        public List<MenuItemBuilder> MenuItems;

        #endregion

        public override MenuItem CreateMenuItem(MenuItem parent) {
            return new MenuItemList(parent, this);
        }


    }

}