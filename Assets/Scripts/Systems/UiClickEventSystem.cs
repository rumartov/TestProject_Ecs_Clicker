using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityComponents;
using UnityEngine.Scripting;

namespace Systems
{
    internal sealed class UiClickEventSystem : EcsUguiCallbackSystem
    {
        private readonly EcsFilter _businessCardFilter;
        private readonly EcsPool<BusinessCard> _businessCardPool;
        private readonly EcsFilter _clickedBusinessCard;
        private readonly EcsPool<Clicked> _clickedPool;
        private readonly EcsFilter _clickedPowerUp;
        private readonly EcsFilter _powerUpFilter;
        private readonly EcsPool<PowerUp> _powerUpPool;
        private readonly EcsWorld _world;

        public UiClickEventSystem(EcsWorld world)
        {
            _world = world;

            _powerUpFilter = _world.Filter<PowerUp>().End();
            _clickedPowerUp = _world.Filter<PowerUp>().Inc<Clicked>().End();

            _powerUpFilter = _world.Filter<PowerUp>().End();
            _businessCardFilter = _world.Filter<BusinessCard>().End();
            _clickedBusinessCard = _world.Filter<BusinessCard>().Inc<Clicked>().End();
            _clickedPowerUp = _world.Filter<PowerUp>().Inc<Clicked>().End();

            _powerUpPool = _world.GetPool<PowerUp>();
            _businessCardPool = _world.GetPool<BusinessCard>();
            _clickedPool = _world.GetPool<Clicked>();
        }

        [Preserve]
        [EcsUguiClickEvent]
        private void OnAnyClick(in EcsUguiClickEvent e)
        {
            SetBusinessCardLevelUpClicked(e);
            SetPowerUpClicked(e);

            SendPowerUpBoughtEvent(e);
            SendBusinessCardLevelUpBoughtEvent(e);
        }

        private void SendBusinessCardLevelUpBoughtEvent(EcsUguiClickEvent e)
        {
            foreach (var index in _clickedBusinessCard)
            {
                e.Sender.TryGetComponent(out LevelUpButtonView levelUpButtonView);

                var businessCardView = levelUpButtonView.BusinessCardView;

                ref var buyLevelUpEvent = ref _world.GetPool<BuyLevelUpEvent>().Add(index);

                buyLevelUpEvent.Id = businessCardView.Id;
            }
        }

        private void SendPowerUpBoughtEvent(EcsUguiClickEvent e)
        {
            foreach (var index in _clickedPowerUp)
            {
                var powerUpView = e.Sender.GetComponent<PowerUpView>();

                ref var buyPowerUpEvent = ref _world.GetPool<BuyPowerUpEvent>().Add(index);

                buyPowerUpEvent.Id = powerUpView.Id;
            }
        }

        private void SetPowerUpClicked(EcsUguiClickEvent e)
        {
            e.Sender.TryGetComponent(out PowerUpView powerUpView);

            if (powerUpView == null)
                return;

            foreach (var index in _powerUpFilter)
            {
                ref var entityPowerUp = ref _powerUpPool.Get(index);

                if (entityPowerUp.Unlocked)
                    continue;

                if (powerUpView.Id == entityPowerUp.Id) _clickedPool.Add(index);
            }
        }

        private void SetBusinessCardLevelUpClicked(EcsUguiClickEvent e)
        {
            e.Sender.TryGetComponent(out LevelUpButtonView levelUpButtonView);

            if (levelUpButtonView == null)
                return;

            var businessCardView = levelUpButtonView.BusinessCardView;

            if (businessCardView == null)
                return;

            foreach (var index in _businessCardFilter)
            {
                ref var entityBusinessCard = ref _businessCardPool.Get(index);
                if (businessCardView.Id == entityBusinessCard.Id) _clickedPool.Add(index);
            }
        }
    }
}