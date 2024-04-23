using System;
using TMPro;
using UnityEngine;
[Serializable]
public struct MissionDisplayData
{
    public string task;
    public string status;
}
public enum MissionType
{
    EatAnimals, AskQuestions
}
public abstract class Missions : MonoBehaviour
{

    [SerializeField] protected MissionDisplayData missionDisplayData;
    [SerializeField] protected MissionType missionType;
    public abstract void BeginMission();
    public abstract void CompleteMission();
    public abstract bool IsMissionTargetAcheived();
    public virtual void DisplayCurrentStatus(Transform missionBox)
    {
        missionBox.Find("txt_MissionTask").GetComponent<TextMeshProUGUI>().text = missionDisplayData.task;
        missionBox.Find("txt_MissionStatus").GetComponent<TextMeshProUGUI>().text = missionDisplayData.status;


    }
    public MissionType GetMissionType() { return missionType; }
    public MissionDisplayData GetMissionDisplayData()
    {
        return missionDisplayData;
    }

}