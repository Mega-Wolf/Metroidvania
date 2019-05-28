using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.Menu {

    public class PrefabMenuItem : MenuItem {

        #region [FinalVariables]

        private GameObject f_prefab;

        #endregion

        #region [Constructors]

        public PrefabMenuItem(MenuItem parent, string path) : base(parent) {
            f_prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            Texture2D previewTexture = AssetPreview.GetAssetPreview(f_prefab);
            //Texture2D previewTexture = AssetPreview.GetMiniThumbnail(f_prefab);

            // TESTING
            if (previewTexture == null) {
                return;
            }

            ItemData = new ItemData(path, Sprite.Create(previewTexture, new Rect(0.0f, 0.0f, previewTexture.width, previewTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
        }

        #endregion

        #region [Override]

        public override void Execute(Menu menu) {
            // TODO
            // implement me!
        }

        #endregion

    }

}