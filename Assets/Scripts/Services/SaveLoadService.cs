using System.Collections.Generic;
using Data;
using StaticData;
using UnityEngine;

namespace Services
{
    internal class SaveLoadService : ISaveLoadService
    {
        private readonly IStaticDataService _staticDataService;

        public string ProgressKey = "PlayerProgress";

        public SaveLoadService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Save(PlayerProgress progress)
        {
            PlayerPrefs.SetString(ProgressKey, progress.ToJson());
        }

        public PlayerProgress Load()
        {
            return PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialized<PlayerProgress>();
        }

        public PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress
            {
                BalanceData =
                {
                    Value = 0
                },
                BusinessCards = new List<BusinessCardData>
                {
                    CreateDefaultBusinessCardData(BusinessTypeId.Business1),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business2),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business3),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business4),
                    CreateDefaultBusinessCardData(BusinessTypeId.Business5)
                }
            };

            return progress;
        }

        private BusinessCardData CreateDefaultBusinessCardData(BusinessTypeId id)
        {
            var businessStaticData = _staticDataService.ForBusiness(id);
            var businessId = businessStaticData.Id;
            var defaultLevel = businessStaticData.DefaultLevel;
            var defaultPrice = businessStaticData.DefaultPrice;
            var defaultIncome = businessStaticData.DefaultIncome;

            var powerUpDataList = CreateDefaultPowerUpData(id);

            return new BusinessCardData
            {
                Id = businessId,
                Level = defaultLevel,
                LevelUpPrice = defaultPrice,
                Income = defaultIncome,
                PowerUps = powerUpDataList
            };
        }

        private List<PowerUpData> CreateDefaultPowerUpData(BusinessTypeId id)
        {
            var powerUpStaticDataList = _staticDataService.ForPowerUps(id);
            var powerUpDataList = new List<PowerUpData>();
            foreach (var powerUpStaticData in powerUpStaticDataList)
            {
                var powerUpData = new PowerUpData
                {
                    Id = powerUpStaticData.Id,
                    BusinessId = powerUpStaticData.BusinessId,
                    Unlocked = false
                };
                powerUpDataList.Add(powerUpData);
            }

            powerUpDataList.Reverse();
            
            return powerUpDataList;
        }
    }
}