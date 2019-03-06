using UnityEngine;
using UnityEngine.UI;

namespace Tools.Menu {

    public class IconVisualMenuItem : VisualMenuItem {

        #region [MemberFields]

        [SerializeField]
        private Image f_image;

        #endregion

        #region [Override]

        public override void Draw(ItemData itemData) {
            f_image.sprite = itemData.Sprite;
            f_image.color = itemData.ForegroundColor;
        }

        #endregion

    }

}