using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    private Missions currentMission;

    [SerializeField] private List<Missions> missions = new List<Missions>();
    [SerializeField] Transform missionBox;
    [SerializeField] Transform panel_Mission;

    private Transform currentMissionDisplayDetails;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {

        BeginNewMission(MissionType.EatAnimals);

    }
    void BeginNewMission(MissionType type)
    {


        currentMission = GetNewMission(type);

        currentMission.BeginMission();

        InstaiateNewMissionDisplayPanel();

    }
    public void CompleteMission()
    {
        //No Need of current Mission info, so simply deallocate the memory

        currentMission.Invoke("CompleteMission", 0.75f);

        currentMission = null;

        currentMissionDisplayDetails.Find("img_CheckBox").GetChild(0).GetComponent<Image>().enabled = true;

        print("Completed");

        if (missions.Count == 0)
        {
            PlayerPrefs.SetInt("missionCompleted", 1);
            // Next level
            return;
        }
        BeginNewMission(missions[0].GetMissionType());

    }
    Missions GetNewMission(MissionType type)
    {
        foreach (var mission in missions)
        {
            if (type == mission.GetMissionType())
            {
                missions.Remove(mission);
                return mission;
            }
        }
        return null;
    }
    void InstaiateNewMissionDisplayPanel()
    {
        currentMissionDisplayDetails = Instantiate(missionBox, panel_Mission);


    }
    private void Update()
    {
        //if no mission, then simply return
        if (currentMission is null) return;

        currentMission.DisplayCurrentStatus(currentMissionDisplayDetails);

        if (currentMission.IsMissionTargetAcheived())
        {
            CompleteMission();
            return;
        }
    }


}
