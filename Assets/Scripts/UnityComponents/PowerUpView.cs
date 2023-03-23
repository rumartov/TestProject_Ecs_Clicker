using Data;
using TMPro;
using UnityEngine;

namespace UnityComponents
{
    internal class PowerUpView : MonoBehaviour
    {
        public PowerUpId Id;
        public TextMeshProUGUI Name;

        public Transform IncomeTransform;
        public TextMeshProUGUI IncomeLabel;
        public TextMeshProUGUI Income;

        public Transform PriceTransform;
        public TextMeshProUGUI PriceLabel;
        public TextMeshProUGUI Price;

        public TextMeshProUGUI BoughtLabel;
    }
}