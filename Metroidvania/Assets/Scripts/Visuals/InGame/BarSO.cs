using System.Collections.Generic;
using UnityEngine;

public class BarSO : ConstScriptableObject {

    public float TimeValueChange;

    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        BarSO other = obj as BarSO;

        return
            TimeValueChange == other.TimeValueChange;
    }

    public override int GetHashCode() {
        return (int)(TimeValueChange * 1000);
    }

}