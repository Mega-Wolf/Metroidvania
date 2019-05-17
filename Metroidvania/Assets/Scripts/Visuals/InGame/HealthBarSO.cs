using System.Collections.Generic;
using UnityEngine;

public class HealthBarSO : ConstScriptableObject {

    public float TimeHealthChange;

    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        HealthBarSO other = obj as HealthBarSO;

        return
            TimeHealthChange == other.TimeHealthChange;
    }

    public override int GetHashCode() {
        return (int)(TimeHealthChange * 1000);
    }

}