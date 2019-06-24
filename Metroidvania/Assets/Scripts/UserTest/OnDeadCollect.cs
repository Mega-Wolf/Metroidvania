using UnityEngine;

public class OnDeadCollect : Singleton<OnDeadCollect> {
    
    #region [MemberFields]

    [SerializeField] private int f_toCollect = 1;

    #endregion

    #region [PrivateVariables]

    private int m_collected = 0;
    private bool m_isQuitting;

    #endregion

    #region [PublicMethods]

    public void RegisterDead(bool isPlayer) {
        ++m_collected;
        if (m_collected == f_toCollect || isPlayer) {
            if (m_isQuitting) {
                return;
            }

            // This calls OnDead when here
            Destroy(gameObject);
        }
    }

    #endregion

}