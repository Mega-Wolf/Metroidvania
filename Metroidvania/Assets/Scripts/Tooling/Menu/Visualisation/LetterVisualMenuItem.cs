using UnityEngine;
using UnityEngine.UI;

namespace Tools.Menu {

    public class LetterVisualMenuItem : VisualMenuItem {

        #region [MemberFields]

        [SerializeField]
        private Text f_text;

        [SerializeField]
        private Image f_backgroundImage;

        #endregion

        #region [Override]

        public override void Draw(ItemData itemData) {
            f_text.text = itemData.Name;
            f_text.color = itemData.ForegroundColor;
            f_backgroundImage.color = itemData.BackgroundColor;
        }

        #endregion

    }

}