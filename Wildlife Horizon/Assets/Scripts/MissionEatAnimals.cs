using TMPro;
using UnityEngine;
public class MissionEatAnimals : Missions
{
    private int maxFoodToAte = 2;
    public override void BeginMission()
    {
    }

    public override void CompleteMission()
    {
        Destroy(this.gameObject);
    }
    public override bool IsMissionTargetAcheived()
    {
        return FoodItemGenerator.totalFoodAte >= maxFoodToAte;
    }

    public override void DisplayCurrentStatus(Transform missionBox)
    {
        //As its dynamic status
        missionDisplayData.status = FoodItemGenerator.totalFoodAte.ToString() + '/' + maxFoodToAte.ToString();
        base.DisplayCurrentStatus(missionBox);

    }


}