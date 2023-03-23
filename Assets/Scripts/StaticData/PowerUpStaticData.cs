using Data;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PowerUpStaticData", menuName = "Static Data/Power Up")]
    public class PowerUpStaticData : ScriptableObject
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessId;
        public string Name;

        public string IncomeLabel = "Income";
        public string PriceLabel = "Price";
        public string BoughtLabel = "Bought";

        public int IncomeMultiplyerPercent;
        public int Price;
    }
}