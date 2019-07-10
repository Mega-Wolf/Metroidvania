using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightRhino : MonoBehaviour {

    public static float SPEED = 1f;
    public static float DOOR_DELTA = 20;

    #region [MemberFields]

    //[SerializeField] private float f_createPercentage = 0;

    [SerializeField] private Color m_disabledDoor;
    [SerializeField] private Color m_enabledDoor;

    [SerializeField] private Door[] f_doors;

    [SerializeField] private GameObject pre_little;

    #endregion

    #region [Init]

    private void Awake() {
        OnValidate();
    }

    private void OnValidate() {
        foreach (Door door in f_doors) {
            door.EnableColor = m_enabledDoor;
            door.DisableColor = m_disabledDoor;

            door.SetEnabled(false);
        }
    }

    #endregion


    #region [PublicMethods]

    public void SetNextStep(Door door, RhinoBoss rhinoBoss) {
        rhinoBoss.gameObject.SetActive(false);

        List<Door> freeDoors = new List<Door>();
        foreach (Door d in f_doors) {
            //if (!d.ContainsPlayer()) {
                freeDoors.Add(d);
            //}
        }

        int rand = Random.Range(0, freeDoors.Count);
        StartCoroutine(StartFromDoor(freeDoors[rand], rhinoBoss));
    }

    #endregion

    #region [PrivateMethods]

    private IEnumerator StartFromDoor(Door door, RhinoBoss rhinoBoss) {

        List<RhinoBoss> spawned = new List<RhinoBoss>();

        // if (Random.value < f_createPercentage) {
        //     for (int i = 0; i < 100; ++i) {
        //         yield return new WaitForFixedUpdate();
        //     }
        //     for (int i = 0; i < 4; ++i) {
        //         GameObject go = Instantiate(pre_little, f_doors[i].SpawnPoint.position, Quaternion.identity);
        //         yield return new WaitForFixedUpdate();
        //         RhinoBoss rb = go.GetComponent<RhinoBoss>();
        //         rb.SwitchState(Random.value > 0.5f);
        //         rb.Velocity = f_doors[i].StartDirection;
        //         go.SetActive(true);
        //         spawned.Add(rb);
        //         rb.SetStartState(rb.ActiveState);
        //     }
        // }

        for (int i = 0; i < DOOR_DELTA; ++i) {
            yield return new WaitForFixedUpdate();
        }

        // foreach (RhinoBoss rb in spawned) {
        //     rb.GroundMovement.SetGroundMask(new string[] { "Default", "MonsterTransparent" });
        // }

        // for (int i = 0; i < 75; ++i) {
        //     yield return new WaitForFixedUpdate();
        // }

        rhinoBoss.Velocity = door.StartDirection;
        rhinoBoss.SwitchState(Random.value > 0.5f);
        rhinoBoss.transform.position = door.SpawnPoint.position;
        rhinoBoss.gameObject.SetActive(true);
    }

    #endregion


    /*

    Sequence:

    - Controlling what the boss rhino does
    - Sending in waves
    - That could probably be modelled as a BT as well?

    */



}