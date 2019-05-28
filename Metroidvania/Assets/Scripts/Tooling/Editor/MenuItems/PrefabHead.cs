namespace Tools.Menu {
    using System.Collections.Generic;
    using UnityEngine;


    public class PrefabHead : MenuItem {

        #region [FinalVariables]

        private PrefabHeadBuilder f_prefabHeadBuilder;

        #endregion

        #region [Constructors]

        public PrefabHead(MenuItem parent, PrefabHeadBuilder prefabHeadBuilder) : base(parent) {
            f_prefabHeadBuilder = prefabHeadBuilder;

            Children = new List<MenuItem>();

            foreach (string directory in System.IO.Directory.GetDirectories(prefabHeadBuilder.Path)) {
                foreach (string file in System.IO.Directory.GetFiles(directory)) {
                    //TODO; filter depending on settings
                    if (file.EndsWith(".meta")) {
                        continue;
                    }
                    Children.Add(new PrefabMenuItem(this, file));
                }
            }

            // those are general things not related to a specific folder
            foreach (string file in System.IO.Directory.GetFiles(prefabHeadBuilder.Path)) {
                if (file.EndsWith(".meta")) {
                    continue;
                }
                Children.Add(new PrefabMenuItem(this, file));
            }

            ItemData = new ItemData(prefabHeadBuilder.Name, Children.Count != 0 ? Color.white : Color.grey ,prefabHeadBuilder.Sprite);

        }

        #endregion

        #region [Override]

        public override void Execute(Menu menu) {
            if (Children.Count > 0) {
                menu.Show(this);
            }
        }

        #endregion

    }

}