using Components;
using Leopotam.EcsLite;
using Services;
using UnityEngine;

namespace Systems
{
    public sealed class IncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticDataService;
        private readonly EcsWorld _world;
        private EcsPool<BusinessCard> _businessCardsPool;
        private EcsFilter _filter;
        private EcsPool<IncomeTimer> _incomeTimers;
        private EcsPool<PowerUp> _powerUpPool;
        private EcsPool<SaveEvent> _saveEventPool;
        private EcsPool<UpdateBalanceViewEvent> _updateBalancePool;
        private EcsPool<UpdateComponentViewEvent> _updateComponentViewPool;

        public IncomeSystem(EcsWorld world, IGameFactory factory, IStaticDataService staticDataService)
        {
            _world = world;
            _factory = factory;
            _staticDataService = staticDataService;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<BusinessCard>().End();

            _incomeTimers = _world.GetPool<IncomeTimer>();

            _businessCardsPool = _world.GetPool<BusinessCard>();
            _updateComponentViewPool = _world.GetPool<UpdateComponentViewEvent>();
            _updateBalancePool = _world.GetPool<UpdateBalanceViewEvent>();
            _powerUpPool = _world.GetPool<PowerUp>();

            _saveEventPool = _world.GetPool<SaveEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter)
            {
                ref var incomeTimer = ref _incomeTimers.Get(index);
                ref var businessCard = ref _businessCardsPool.Get(index);

                if (businessCard.Level <= 0) return;

                incomeTimer.Timer -= Time.deltaTime;

                if (incomeTimer.Timer <= 0)
                {
                    businessCard.Income = businessCard.CalculateBusinessIncome(_staticDataService);

                    _factory.Balance.Value += businessCard.Income;

                    _updateBalancePool.Add(_world.NewEntity());

                    _updateComponentViewPool.SendUpdateComponentEvent(businessCard.EntityId);

                    _saveEventPool.SendSaveEvent(businessCard.EntityId);

                    ResetTimer(ref businessCard, ref incomeTimer);
                }
            }
        }

        private void ResetTimer(ref BusinessCard businessCard, ref IncomeTimer incomeTimer)
        {
            incomeTimer.Timer = businessCard.IncomeDelay;
        }
    }
}