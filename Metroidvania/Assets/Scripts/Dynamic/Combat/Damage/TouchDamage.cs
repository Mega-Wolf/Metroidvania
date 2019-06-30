using UnityEngine;
using System.Collections.Generic;

public class TouchDamage : MonoBehaviour {

    #region [FinalVariables]

    [SerializeField] private Collider2D f_collider;
    [SerializeField] private Collider2D f_collider2;

    //TODO; only use that (but that would break links)
    [SerializeField] private Collider2D[] f_colliders;

    [SerializeField][EnumFlag] private EDamageReceiver f_eDamageReceiver;

    [SerializeField] private int f_damage;

    [SerializeField] private bool f_destroySelf;

    #endregion

    #region [Init]

    private void Start() {
        if (f_colliders == null || f_colliders.Length == 0) {
            f_colliders = new Collider2D[2];
            f_colliders[0] = f_collider;
            f_colliders[1] = f_collider2;
        }
    }

    #endregion

    #region [Updates]

    private void FixedUpdate() {
        bool hittedSth = false;

        if (f_colliders != null && f_colliders.Length != 0) {
            List<Collider2D> colliderList = DamageHelper.ContactList;
            ContactFilter2D cf = new ContactFilter2D();
            cf.SetLayerMask((int)f_eDamageReceiver);
            cf.useLayerMask = true;

            HashSet<Collider2D> colliderSet = new HashSet<Collider2D>();
            List<(Collider2D self, Collider2D other)> colliderCombos = new List<(Collider2D, Collider2D)>();

            for (int i = 0; i < f_colliders.Length; ++i) {
                if (f_colliders[i] == null) {
                    continue;
                }
                f_colliders[i].OverlapCollider(cf, colliderList);
                for (int j = 0; j < colliderList.Count; ++j) {
                    if (colliderSet.Add(colliderList[j])) {
                        colliderCombos.Add((f_colliders[i], colliderList[j]));
                    }
                }
            }

            for (int i = 0; i < colliderCombos.Count; ++i) {
                hittedSth = true;
                IDamageTaker health = colliderCombos[i].other.GetComponent<IDamageTaker>();
                if (health != null) {
                    bool shallSpawn = true;
                    if (health.Controller is Player p) {
                        shallSpawn = !(p.ActiveStackedState is CharacterHitted);
                    }
                    if (shallSpawn) {
                        Instantiate(Consts.Instance.PreHit, (colliderCombos[i].self.transform.position + colliderCombos[i].other.transform.position) / 2f, Quaternion.identity);
                    }

                    //TODO; this should still bump the toucher backwards a bit
                    //TODO; Also, I would still actually want the damaged HashSet since otherwise I kill other creatures very fast
                    health.TakeDamage(f_damage, Vector2.zero);

                }
            }


        } else {
            Debug.LogWarning("HERE");
            //TODO; this is not reached anymore


            List<Collider2D> colliderList = DamageHelper.ContactList;
            ContactFilter2D cf = new ContactFilter2D();
            cf.SetLayerMask((int)f_eDamageReceiver);
            cf.useLayerMask = true;

            f_collider.OverlapCollider(cf, colliderList);

            //TODO; this is ugly and unnecessary if the other one is enabled
            if (colliderList.Count == 0) {
                if (f_collider2 != null) {
                    f_collider2.OverlapCollider(cf, colliderList);
                }
            }

            for (int i = 0; i < colliderList.Count; ++i) {
                hittedSth = true;
                IDamageTaker health = colliderList[i].GetComponent<IDamageTaker>();
                if (health != null) {
                    //TODO; this should still bump the toucher backwards a bit
                    //TODO; Also, I would still actually want the damaged HashSet since otherwise I kill other creatures very fast
                    health.TakeDamage(f_damage, Vector2.zero);
                }
            }
        }

        if (f_destroySelf && hittedSth) {
            Destroy(gameObject);
        }
    }

    #endregion

}