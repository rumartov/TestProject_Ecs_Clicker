using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "HudStaticData", menuName = "Static Data/Hud")]
    public class HudStaticData : ScriptableObject
    {
        public string BalanceLabel = "Balance";
    }
}