using System.Collections.Generic;
using Components;
using Data;
using Leopotam.EcsLite;
using UnityComponents;
using UnityEngine;

namespace Services
{
    internal class GameFactory : IGameFactory
    {
        private readonly PlayerProgress _progress;
        private readonly IStaticDataService _staticDataService;
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;

        public GameFactory(EcsWorld world, UiRoot uiRootRoot, PlayerProgress progress,
            IStaticDataService staticDataService)
        {
            _world = world;
            _uiRoot = uiRootRoot;
            _progress = progress;
            _staticDataService = staticDataService;
        }

        public Balance Balance { get; set; }

        public void CreateBalance()
        {
            Balance = new Balance
            {
                Value = _progress.BalanceData.Value
            };

            var balanceView = _uiRoot.hud.GetComponentInChildren<BalanceView>();
            balanceView.BalanceLabel.text = _staticDataService.ForHud().BalanceLabel;
            balanceView.Value.text = Balance.Value.ToString();
        }

        public void CreateBusinessCards()
        {
            var businessCards = _progress.BusinessCards;
            foreach (var businessCardFromSave in businessCards) CreateBusinessCard(businessCardFromSave);
        }

        private void CreateBusinessCard(BusinessCardData businessCardFromSave)
        {
            var entity = _world.NewEntity();
            ref var businessCard = ref _world.GetPool<BusinessCard>().Add(entity);

            var businessStaticData = _staticDataService.ForBusiness(businessCardFromSave.Id);

            businessCard.EntityId = entity;
            businessCard.Id = businessCardFromSave.Id;
            businessCard.Level = businessCardFromSave.Level;
            businessCard.Income = businessCardFromSave.Income;
            businessCard.LevelUpPrice = businessCardFromSave.LevelUpPrice;
            businessCard.PowerUps = new List<EcsPackedEntityWithWorld>();

            businessCard.IncomeDelay = businessStaticData.IncomeDelay;

            var businessCardObj = (GameObject) Object.Instantiate(Resources.Load(AssetPath.BusinessCard),
                _uiRoot.businessCardContainer);

            var businessCardView = businessCardObj.GetComponent<BusinessCardView>();

            businessCardView.Name.text = businessStaticData.Name;
            businessCardView.LevelUpPriceLabel.text = businessStaticData.LevelUpPriceLabel;
            businessCardView.IncomeLabel.text = businessStaticData.IncomeLabel;
            businessCardView.LevelLabel.text = businessStaticData.LevelLabel;

            businessCardView.Id = businessCard.Id;
            businessCardView.Level.text = businessCard.Level.ToString();
            businessCardView.Income.text = businessCard.Income.ToString();
            businessCardView.LevelUpPrice.text = businessCard.LevelUpPrice.ToString();

            CreatePowerUps(businessCardFromSave, businessCardView, ref businessCard);

            var incomeTimer = _world.GetPool<IncomeTimer>().Add(entity);
            incomeTimer.Timer = businessCard.IncomeDelay;
        }

        private void CreatePowerUps(BusinessCardData businessCardFromSave, BusinessCardView businessCardView,
            ref BusinessCard businessCard)
        {
            var powerUps = businessCardFromSave.PowerUps;
            
            foreach (var powerUpFromSave in powerUps)
                CreatePowerUp(businessCardView, powerUpFromSave, ref businessCard);
        }

        private void CreatePowerUp(BusinessCardView businessCardView, PowerUpData powerUpFromSave,
            ref BusinessCard businessCard)
        {
            var entity = _world.NewEntity();
            ref var powerUp = ref _world.GetPool<PowerUp>().Add(entity);

            var powerUpStaticData = _staticDataService.ForPowerUp(powerUpFromSave.Id);

            powerUp.Id = powerUpFromSave.Id;
            powerUp.BusinessId = powerUpFromSave.BusinessId;
            powerUp.Unlocked = powerUpFromSave.Unlocked;

            powerUp.Price = powerUpStaticData.Price;
            powerUp.IncomeMultiplyerPercent = powerUpStaticData.IncomeMultiplyerPercent;

            var powerUpObj = (GameObject) Object.Instantiate(Resources.Load(AssetPath.PowerUp),
                businessCardView.PowerUpRoot);

            var powerUpView = powerUpObj.GetComponent<PowerUpView>();

            powerUpView.Id = powerUpFromSave.Id;

            powerUpView.Name.text = powerUpStaticData.Name;
            powerUpView.IncomeLabel.text = powerUpStaticData.IncomeLabel;
            powerUpView.PriceLabel.text = powerUpStaticData.PriceLabel;

            powerUpView.Income.text = powerUpStaticData.IncomeMultiplyerPercent.ToString();
            powerUpView.Price.text = powerUpStaticData.Price.ToString();

            var packed = _world.PackEntityWithWorld(entity);

            businessCard.PowerUps.Add(packed);

            if (powerUp.Unlocked)
                _world.GetPool<Unlocked>().Add(entity);
        }
    }
}