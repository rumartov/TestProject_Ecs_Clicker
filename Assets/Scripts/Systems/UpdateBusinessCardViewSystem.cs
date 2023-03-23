using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.EcsLite;
using UnityComponents;

namespace Systems
{
    public sealed class UpdateBusinessCardViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;
        private BalanceView _balanceView;
        private EcsPool<BusinessCard> _businessCardPool;
        private List<BusinessCardView> _businessCardViewList;
        private EcsFilter _filter;

        public UpdateBusinessCardViewSystem(EcsWorld world, UiRoot uiRoot)
        {
            _world = world;
            _uiRoot = uiRoot;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = _world.Filter<BusinessCard>().Inc<UpdateComponentViewEvent>().End();
            _businessCardPool = _world.GetPool<BusinessCard>();
            _businessCardViewList = _uiRoot.hud.GetComponentsInChildren<BusinessCardView>().ToList();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _filter)
            {
                ref var businessCard = ref _businessCardPool.Get(index);
                foreach (var businessCardView in _businessCardViewList)
                    if (businessCardView.Id == businessCard.Id)
                    {
                        businessCardView.Income.text = businessCard.Income.ToString();
                        businessCardView.Level.text = businessCard.Level.ToString();
                        businessCardView.LevelUpPrice.text = businessCard.LevelUpPrice.ToString();
                    }
            }
        }
    }
}