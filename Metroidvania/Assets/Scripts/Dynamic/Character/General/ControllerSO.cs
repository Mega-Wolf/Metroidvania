using UnityEngine;

public class ControllerSO : ConstScriptableObject {

    #region [MemberFields]

    [SerializeField] private float extends = 0.1f;
    [SerializeField] private float extraRayLength = 0.1f;

    [SerializeField] private int maxCollisionIterations = 3;
    [SerializeField] private float impactLength = 1/3f;

    #endregion

    #region [Properties]

    public float EXTENDS { get { return extends; } }
    public float EXTRA_RAY_LENGTH { get { return extraRayLength; } }

    public int MAX_COLLISION_ITERATIONS { get { return maxCollisionIterations; } }
    public float IMPACT_LENGTH { get { return impactLength; } }

    #endregion

}