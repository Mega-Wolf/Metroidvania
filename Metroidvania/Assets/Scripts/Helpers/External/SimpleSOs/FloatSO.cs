using UnityEngine;

public class FloatSO : ScriptableObject {

    public static implicit operator float (FloatSO floatSO) {
        return floatSO.f_value;
    }

    [SerializeField]
    private float f_value;

}