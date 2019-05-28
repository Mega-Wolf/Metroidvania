using UnityEngine;

namespace Tools.Menu {

    public abstract class VisualMenuItem : MonoBehaviour {

        #region [PublicMethods]

        public abstract void Draw(ItemData itemData);

        #endregion

    }

}