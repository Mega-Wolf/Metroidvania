using UnityEngine;

public abstract class Movement {

    #region [Properties]

    public abstract LayerMask Mask { get; }

    #endregion

    #region [PublicMethods]

    public abstract void AirMove();

    #endregion

}