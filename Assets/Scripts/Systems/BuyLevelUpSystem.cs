using Components;
using Leopotam.EcsLite;
using Services;
using StaticData;

namespace Systems
{
    internal class BuyLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticDataService;
        private readonly EcsWorld _world;
        private EcsPool<BusinessCard> _businessCardPool;
        private EcsFilter _eventFilter;
        private EcsPool<UpdateComponentViewEvent> _updateComponentViewPool;

        public BuyLevelUpSystem(EcsWorld world, IGameFactory factory, IStaticDataService staticDataService)
        {
            _world = world;
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public void Init(IEcsSystems systems)
        {
            _eventFilter = _world.Filter<BusinessCard>().Inc<BuyLevelUpEvent>().End();
            _businessCardPool = _world.GetPool<BusinessCard>();
            _updateComponentViewPool = _world.GetPool<UpdateComponentViewEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _eventFilter)
            {
                ref var businessCard = ref _businessCardPool.Get(index);

                var levelUpPrice = businessCard.LevelUpPrice;

                var balance = _factory.Balance;
                if (!balance.HasEnoughBalance(levelUpPrice)) return;

                balance.SpendBalance(levelUpPrice);

                LevelUpBusinessCard(ref businessCard);

                _updateComponentViewPool.SendUpdateComponentEvent(businessCard.EntityId);
            }
        }

        private void LevelUpBusinessCard(ref BusinessCard businessCard)
        {
            var businessStaticData = _staticDataService.ForBusiness(businessCard.Id);

            businessCard.LevelUpPrice = CalculateBusinessCardLevelUpPrice(businessCard, businessStaticData);
            businessCard.Level++;
            businessCard.Income = businessCard.CalculateBusinessIncome(_staticDataService);
        }

        private static int CalculateBusinessCardLevelUpPrice(BusinessCard businessCard,
            BusinessStaticData businessStaticData)
        {
            return businessCard.Level * businessStaticData.DefaultPrice;
        }
    }
}