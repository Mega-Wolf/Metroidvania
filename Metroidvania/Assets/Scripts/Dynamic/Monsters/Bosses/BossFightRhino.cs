using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightRhino : MonoBehaviour {

    #region [MemberFields]

    [SerializeField] private Color m_disabledDoor;
    [SerializeField] private Color m_enabledDoor;

    [SerializeField] private Door[] f_doors;

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook]
    private RhinoBoss f_rhinoBoss;

    #endregion

    #region [Init]

    private void OnValidate() {
        foreach (Door door in f_doors) {
            door.EnableColor = m_enabledDoor;
            door.DisableColor = m_disabledDoor;

            door.SetEnabled(false);
        }
    }

    #endregion


    #region [PublicMethods]

    public void SetNextStep(Door door) {
        f_rhinoBoss.gameObject.SetActive(false);

        List<Door> freeDoors = new List<Door>();
        foreach (Door d in f_doors) {
            if (!d.ContainsPlayer()) {
                freeDoors.Add(d);
            }
        }

        int rand = Random.Range(0, freeDoors.Count);
        StartCoroutine(StartFromDoor(freeDoors[rand]));
    }

    #endregion

    #region [PrivateMethods]

    private IEnumerator StartFromDoor(Door door) {

        for (int i = 0; i < 100; ++i) {
            yield return new WaitForFixedUpdate();
        }

        f_rhinoBoss.SwitchState(Random.value > 0.5f);
        f_rhinoBoss.transform.position = door.SpawnPoint.position;
        f_rhinoBoss.Velocity = door.StartDirection;
        f_rhinoBoss.gameObject.SetActive(true);
    }

    #endregion


    /*

    Sequence:

    - Controlling what the boss rhino does
    - Sending in waves
    - That could probably be modelled as a BT as well?

    */



}