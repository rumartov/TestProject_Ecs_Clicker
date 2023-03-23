using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.EcsLite;
using Services;
using UnityComponents;

namespace Systems
{
    public sealed class UpdatePowerUpViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IGameFactory _factory;
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;
        private BalanceView _balanceView;
        private EcsFilter _filter;
        private EcsPool<PowerUp> _powerUpPool;
        private List<PowerUpView> _powerUpViewList;

        public UpdatePowerUpViewSystem(EcsWorld world, IGameFactory factory, UiRoot uiRoot)
        {
            _world = world;
            _factory = factory;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<PowerUp>().Inc<Unlocked>().End();
            _balanceView = _uiRoot.hud.GetComponentInChildren<BalanceView>();
            _powerUpViewList = _uiRoot.hud.GetComponentsInChildren<PowerUpView>().ToList();
            _powerUpPool = _world.GetPool<PowerUp>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter)
            {
                ref var powerUp = ref _powerUpPool.Get(index);

                foreach (var powerUpView in _powerUpViewList)
                    if (powerUpView.Id == powerUp.Id)
                    {
                        powerUpView.IncomeTransform.gameObject.SetActive(false);
                        powerUpView.PriceTransform.gameObject.SetActive(false);
                        powerUpView.BoughtLabel.gameObject.SetActive(true);
                    }

                _balanceView.Value.text = _factory.Balance.Value.ToString();
            }
        }
    }
}