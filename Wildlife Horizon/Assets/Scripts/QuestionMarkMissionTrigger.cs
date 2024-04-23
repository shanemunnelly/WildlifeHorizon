using System;
using UnityEngine;

public class QuestionMarkMissionTrigger : MonoBehaviour
{
    public static event Action eventOnQuestionMarkTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            eventOnQuestionMarkTriggered?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}