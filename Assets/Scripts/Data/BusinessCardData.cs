﻿using System;
using System.Collections.Generic;
using StaticData;

namespace Data
{
    [Serializable]
    public class BusinessCardData
    {
        public BusinessTypeId Id;
        public int Level;
        public float Income;
        public int LevelUpPrice;
        public List<PowerUpData> PowerUps;
    }
}