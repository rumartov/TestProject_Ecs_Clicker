using Components;
using Leopotam.EcsLite;
using Services;
using UnityComponents;

namespace Systems
{
    public sealed class UpdateBalanceViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IGameFactory _factory;
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;
        private BalanceView _balanceView;
        private EcsFilter _filter;

        public UpdateBalanceViewSystem(EcsWorld world, IGameFactory factory, UiRoot uiRoot)
        {
            _world = world;
            _factory = factory;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<UpdateComponentViewEvent>().End();
            _balanceView = _uiRoot.hud.GetComponentInChildren<BalanceView>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter) _balanceView.Value.text = _factory.Balance.Value.ToString();
        }
    }
}