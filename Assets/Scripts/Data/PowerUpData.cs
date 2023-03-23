using System;
using StaticData;

namespace Data
{
    [Serializable]
    public class PowerUpData
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessId;
        public bool Unlocked;
    }
}