using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Services;

public static class Extentions
{
    public static bool HasEnoughBalance(this Balance balance, float price)
    {
        return price <= balance.Value;
    }

    public static void SpendBalance(this Balance balance, float price)
    {
        balance.Value -= price;
    }

    public static float CalculateBusinessIncome(this ref BusinessCard businessCard,
        IStaticDataService staticDataService)
    {
        var businessStaticData = staticDataService.ForBusiness(businessCard.Id);

        return businessCard.Level
               * businessStaticData.DefaultIncome
               * (1 + businessCard.GetUnlockedPowerUpsMultiplyer());
    }

    private static float GetUnlockedPowerUpsMultiplyer(this ref BusinessCard businessCard)
    {
        float powerUpsMultiplyer = 0;

        var businessCardPowerUps = UnpackPowerUps(ref businessCard);

        foreach (var powerUp in businessCardPowerUps)
        {
            var pwrUp = powerUp;
            if (pwrUp.Unlocked) powerUpsMultiplyer += pwrUp.GetIncomeMultiplyerPercentage();
        }


        return powerUpsMultiplyer;
    }

    private static List<PowerUp> UnpackPowerUps(ref BusinessCard businessCard)
    {
        var businessCardPowerUps = new List<PowerUp>();
        foreach (var packedEntity in businessCard.PowerUps)
        {
            packedEntity.Unpack(out var world, out var entity);
            ref var powerUp = ref world.GetPool<PowerUp>().Get(entity);
            businessCardPowerUps.Add(powerUp);
        }

        return businessCardPowerUps;
    }

    public static float GetIncomeMultiplyerPercentage(this PowerUp powerUp)
    {
        var powerUpIncomeMultiplyerPercent = 1f + powerUp.IncomeMultiplyerPercent / 100f;
        return powerUpIncomeMultiplyerPercent;
    }

    public static void SendUpdateComponentEvent(this EcsPool<UpdateComponentViewEvent> updateComponentViewPool,
        int entityId)
    {
        if (!updateComponentViewPool.Has(entityId))
            updateComponentViewPool.Add(entityId);
    }

    public static void SendSaveEvent(this EcsPool<SaveEvent> savePool,
        int entityId)
    {
        if (!savePool.Has(entityId))
            savePool.Add(entityId);
    }
}