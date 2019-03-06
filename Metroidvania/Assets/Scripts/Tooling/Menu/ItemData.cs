using UnityEngine;

namespace Tools.Menu {

    public class ItemData {

        public string Name;
        public Color ForegroundColor;
        public Color BackgroundColor;
        public Sprite Sprite;

        public ItemData(string name, Color foregroundColor, Color backgroundColor, Sprite sprite) {
            Name = name;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Sprite = sprite;
        }

        public ItemData(string name, Sprite sprite) : this(name, Color.white, Color.black, sprite) { }

        public ItemData(string name, Color foregroundColor, Sprite sprite) : this(name, foregroundColor, Color.black, sprite) { }

    }

}