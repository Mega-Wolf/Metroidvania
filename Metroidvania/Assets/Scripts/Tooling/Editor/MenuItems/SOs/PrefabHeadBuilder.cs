using System.Collections.Generic;
using UnityEngine;

namespace Tools.Menu {

    [CreateAssetMenu(fileName = "PrefabHead", menuName = "Metroidvania/MenuItems/PrefabHead", order = 1)]
    public class PrefabHeadBuilder : MenuItemBuilder {

        #region [MemberFields]

        public string Name;

        public string Path;

        public Sprite Sprite;

        public List<Menu> MenusToShow;

        #endregion

        #region [Override]

        public override MenuItem CreateMenuItem(MenuItem parent) {
            return new PrefabHead(parent, this);
        }

        #endregion

    }

}