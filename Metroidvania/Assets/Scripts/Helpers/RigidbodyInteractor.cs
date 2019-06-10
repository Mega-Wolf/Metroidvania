using NaughtyAttributes;
using UnityEngine;

public class RigidbodyInteractor : MonoBehaviour {
    
    public Vector2 velocity;

    [Button]
    public void SetVelocity() {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

}