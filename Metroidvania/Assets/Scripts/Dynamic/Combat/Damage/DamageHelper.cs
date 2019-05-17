using System.Collections.Generic;
using UnityEngine;

public enum EDamageReceiver {
    Player = 1 << 8,
    Enemy = 1 << 9,
    Environment = 1 << 10,
}

public static class DamageHelper {

    public static List<ContactPoint2D> ContactList { get; } = new List<ContactPoint2D>(10);

}