using System.Collections.Generic;
using Leopotam.EcsLite;
using StaticData;

namespace Components
{
    public struct BusinessCard
    {
        public int EntityId;
        public BusinessTypeId Id;
        public int Level;
        public float Income;
        public float IncomeDelay;
        public int LevelUpPrice;
        public List<EcsPackedEntityWithWorld> PowerUps;
    }
}