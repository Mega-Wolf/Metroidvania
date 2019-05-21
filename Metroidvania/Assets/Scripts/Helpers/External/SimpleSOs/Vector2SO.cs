using UnityEngine;

public class Vector2SO : ScriptableObject {

    public static implicit operator Vector2 (Vector2SO vector2SO) {
        return vector2SO.f_value;
    }

    [SerializeField]
    private Vector2 f_value;

}