using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
struct QuizQuestionDisplay
{
    public TextMeshProUGUI txtquestionNumber, txtquestionStatement;
    public TextMeshProUGUI[] txtanswerOptions;
    public List<Button> btnOptions;
}
[Serializable]
struct QuizQuestions
{
    public string questionStatement;
    public List<string> options;
    public int answerInd;
}
public class MissionAskQuestions : Missions
{
    [SerializeField] Transform barrelToInteract;
    [SerializeField] Transform canvasQuestions;
    [SerializeField] int totalQuestions = 3;
    [SerializeField] private int askedQuestions = 0;
    public static event Action eventOnMissionCompleted;
    private void OnEnable()
    {
        QuestionMarkMissionTrigger.eventOnQuestionMarkTriggered += OnQestionMarkTriggered;
    }
    private void OnDisable()
    {
        QuestionMarkMissionTrigger.eventOnQuestionMarkTriggered -= OnQestionMarkTriggered;

    }
    [SerializeField] List<QuizQuestions> quizQuestions;
    [SerializeField] QuizQuestionDisplay quizQuestionDisplay;

    void OnQestionMarkTriggered()
    {
        canvasQuestions.gameObject.SetActive(true);
        AskQuestion();
    }
    public override void BeginMission()
    {
        barrelToInteract.gameObject.SetActive(true);
    }
    public void ClickOnOption(int ind)
    {
        for (int i = 0; i < quizQuestionDisplay.btnOptions.Count; i++)
        {
            quizQuestionDisplay.btnOptions[i].interactable = false;
            if (quizQuestions[askedQuestions].answerInd != i)
                quizQuestionDisplay.btnOptions[i].GetComponent<Image>().color = Color.red;
            else
                quizQuestionDisplay.btnOptions[i].GetComponent<Image>().color = Color.green;

        }
        askedQuestions++;

        if (askedQuestions < totalQuestions)
            Invoke("AskQuestion", 0.75f);

    }
    public void AskQuestion()
    {
        for (int i = 0; i < quizQuestionDisplay.btnOptions.Count; i++)
        {
            quizQuestionDisplay.btnOptions[i].interactable = true;

            quizQuestionDisplay.btnOptions[i].GetComponent<Image>().color = Color.white;

        }
        quizQuestionDisplay.txtquestionNumber.text = "Question # " + (askedQuestions + 1);
        quizQuestionDisplay.txtquestionStatement.text = quizQuestions[askedQuestions].questionStatement;

        int totalOptions = quizQuestions[askedQuestions].options.Count;
        for (int i = 0; i < totalOptions; i++)
        {
            quizQuestionDisplay.txtanswerOptions[i].text = quizQuestions[askedQuestions].options[i];
        }
    }
    public override void CompleteMission()
    {

        canvasQuestions.gameObject.SetActive(false);

        eventOnMissionCompleted?.Invoke();

        Destroy(this.gameObject);
    }

    public override void DisplayCurrentStatus(Transform missionBox)
    {
        missionDisplayData.status = askedQuestions.ToString() + '/' + totalQuestions.ToString();

        base.DisplayCurrentStatus(missionBox);
    }

    public override bool IsMissionTargetAcheived()
    {
        return askedQuestions >= totalQuestions;
    }
}