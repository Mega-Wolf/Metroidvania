using UnityEngine;

public class IntSO : ScriptableObject {

    public static implicit operator int (IntSO intSO) {
        return intSO.f_value;
    }

    [SerializeField]
    private int f_value;

}