using UnityEngine;

public class StringSO : ScriptableObject {

    public static implicit operator string(StringSO stringSO) {
        return stringSO.f_value;
    }

    [SerializeField]
    private string f_value;

}