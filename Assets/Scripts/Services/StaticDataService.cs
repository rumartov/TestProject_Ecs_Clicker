using System.Collections.Generic;
using System.Linq;
using Data;
using StaticData;
using UnityEngine;

namespace Services
{
    internal class StaticDataService : IStaticDataService
    {
        private const string BusinessesPath = "Static Data/Businesses";
        private const string HudPath = "Static Data/Hud/HudStaticData";
        private const string PowerUpsPath = "Static Data/PowerUps";

        private Dictionary<BusinessTypeId, BusinessStaticData> _businesses;
        private HudStaticData _hudStaticData;
        private Dictionary<PowerUpId, PowerUpStaticData> _powerUps;


        public void Load()
        {
            _businesses = Resources
                .LoadAll<BusinessStaticData>(BusinessesPath)
                .ToDictionary(x => x.Id, x => x);

            _powerUps = Resources
                .LoadAll<PowerUpStaticData>(PowerUpsPath)
                .ToDictionary(x => x.Id, x => x);

            _hudStaticData = Resources.Load<HudStaticData>(HudPath);
        }

        public BusinessStaticData ForBusiness(BusinessTypeId businessCardId)
        {
            return _businesses.TryGetValue(businessCardId, out var staticData)
                ? staticData
                : null;
        }

        public HudStaticData ForHud()
        {
            return _hudStaticData;
        }

        public PowerUpStaticData ForPowerUp(PowerUpId powerUpId)
        {
            return _powerUps.TryGetValue(powerUpId, out var staticData)
                ? staticData
                : null;
        }

        public List<PowerUpStaticData> ForPowerUps(BusinessTypeId businessTypeId)
        {
            var powerUpStaticDataList = new List<PowerUpStaticData>();
            foreach (var powerUpStaticData in _powerUps.Values)
                if (powerUpStaticData.BusinessId == businessTypeId)
                    powerUpStaticDataList.Add(powerUpStaticData);

            powerUpStaticDataList.Reverse();
            return powerUpStaticDataList;
        }
    }
}