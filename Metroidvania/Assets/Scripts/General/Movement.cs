using UnityEngine;

public abstract class Movement {

    #region [Properties]

    public abstract LayerMask Mask { get; }

    #endregion

    #if UNITY_EDITOR
    public abstract void OnDrawGizmos();
    #endif

    #region [PublicMethods]

    public abstract void AirMove();

    #endregion

}