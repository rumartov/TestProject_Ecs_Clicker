using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public BalanceData BalanceData;
        public List<BusinessCardData> BusinessCards;

        public PlayerProgress()
        {
            BalanceData = new BalanceData();
            BusinessCards = new List<BusinessCardData>();
        }
    }
}