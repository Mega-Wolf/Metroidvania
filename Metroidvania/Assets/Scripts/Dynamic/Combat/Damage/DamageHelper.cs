using System.Collections.Generic;
using UnityEngine;



public static class DamageHelper {

    public static List<Collider2D> ContactList { get; } = new List<Collider2D>(10);

    public static ContactFilter2D PlayerFilter { get; } = new ContactFilter2D() { useLayerMask = true, layerMask = (int)EDamageReceiver.Player };

}