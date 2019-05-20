using UnityEngine;

public partial class Consts {

    public PlayerSO PlayerSO;

}

public class PlayerSO : ConstScriptableObject {

    #region [MemberFields]

    [Header("Other ConstSOs")]
    [SerializeField] private PlayerAirSO playerAir;

    [Header("Basics")]
    [SerializeField] private int health;

    [Header("Movements")]
    [SerializeField] private float walkSpeed;

    [Header("HitSide")]
    [SerializeField] private int sideDamage;

    [Header("HitUp")]
    [SerializeField] private int upDamage;

    [Header("HitDown")]
    [SerializeField] private int downDamage;

    #endregion

    #region [Properties]

    public PlayerAirSO PLAYER_AIR { get { return playerAir; } }
    public int HEALTH { get { return health; } }
    public float WALK_SPEED { get { return walkSpeed; } }

    public int SIDE_DAMAGE { get { return sideDamage; } }
    public int UP_DAMAGE { get { return upDamage; } }
    public int DOWN_DAMAGE { get { return downDamage; } }


    #endregion

}