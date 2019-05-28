using UnityEngine;

public class MeteorSpawn : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField, Autohook] private Collider2D f_collider;

    private int f_framesBetweenSpawns = 10 * 50;

    #endregion

    #region [MemberFields]

    [SerializeField] private GameObject f_prefab;

    #endregion

    #region [PrivateVariables]

    private int m_currentFrame;

    #endregion

    #region [Updates]

    private void Update() {
        if (m_currentFrame == f_framesBetweenSpawns) {
            m_currentFrame = 0;
            if (f_framesBetweenSpawns > 5 * 50) {
                f_framesBetweenSpawns -= 10;
            }
            SpawnRock();
        } else {
            ++m_currentFrame;
        }
    }

    #endregion

    #region [PublicMethods]

    public void SpawnRock() {
        GameObject go = Instantiate(f_prefab, new Vector3(Random.Range(f_collider.bounds.min.x, f_collider.bounds.max.x), transform.position.y, 0), Quaternion.identity, transform);
        RhinoBoss rb = go.GetComponent<RhinoBoss>();
        rb.SwitchState(true);
        rb.Velocity = 3 * Vector2.down;
    }

    #endregion

}