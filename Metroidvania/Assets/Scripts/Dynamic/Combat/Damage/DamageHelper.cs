using System.Collections.Generic;
using UnityEngine;

public enum EDamageReceiver {
    Player = 1 << 8,
    Enemy = 1 << 9,
    Environment = 1 << 10,
}

public static class DamageHelper {

    public static List<Collider2D> ContactList { get; } = new List<Collider2D>(10);

    public static ContactFilter2D PlayerFilter { get; } = new ContactFilter2D() { useLayerMask = true, layerMask = (int)EDamageReceiver.Player };

}