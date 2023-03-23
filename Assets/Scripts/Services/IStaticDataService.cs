using System.Collections.Generic;
using Data;
using StaticData;

namespace Services
{
    public interface IStaticDataService
    {
        void Load();
        BusinessStaticData ForBusiness(BusinessTypeId businessCardId);
        HudStaticData ForHud();
        PowerUpStaticData ForPowerUp(PowerUpId powerUpId);
        List<PowerUpStaticData> ForPowerUps(BusinessTypeId businessTypeId);
    }
}