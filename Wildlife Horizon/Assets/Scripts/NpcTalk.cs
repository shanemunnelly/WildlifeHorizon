using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcTalk : MonoBehaviour
{
    [SerializeField] string[] dialouges;
    private int dInd;
    [SerializeField] Transform player;
    Animator animator;
    [SerializeField] float radius = 2f;
    int ind = 0;
    [SerializeField] playermovement playermovement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Transform panelDialougeBeforeMission;
    [SerializeField] Transform panelDialougeAfterMission;
    [SerializeField] Transform interactButton;
    [SerializeField] TextMeshProUGUI txtinteractButton;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform missionManager;
    bool playerInSight;
    bool isInTalkMode;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        MissionAskQuestions.eventOnMissionCompleted += ChangeDialouge;
    }
    private void OnDisable()
    {

        MissionAskQuestions.eventOnMissionCompleted -= ChangeDialouge;
    }

    void ChangeDialouge()
    {
        ind++;
    }

    private void Update()
    {

        playerInSight = Physics.CheckSphere(this.transform.position, radius, playerLayer);
        if (playerInSight)
        {
            if (isInTalkMode)
            {
                mouseLook.enabled = false;
                playermovement.enabled = false;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!NextDialouge())
                    {
                        isInTalkMode = false;
                    }

                }
            }
            if (Vector3.Distance(this.transform.position, player.transform.position) > 0.75f)
                this.transform.LookAt(player);
            interactButton.gameObject.SetActive(true);
            txtinteractButton.text = "Press Y to Talk!";
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (PlayerPrefs.GetInt("missionCompleted", 0) == 0)
                {
                    isInTalkMode = true;
                    panelDialougeBeforeMission.gameObject.SetActive(true);
                    //missionManager.gameObject.SetActive(true);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    panelDialougeAfterMission.gameObject.SetActive(true);
                }
                animator.SetBool("isTalk", true);
            }

        }

        else
        {

            panelDialougeBeforeMission.gameObject.SetActive(false);
            panelDialougeAfterMission.gameObject.SetActive(false);
            interactButton.gameObject.SetActive(false);

            animator.SetBool("isTalk", false);

        }
    }
    bool NextDialouge()
    {

        if (dInd >= dialouges.Length)
        {
            panelDialougeBeforeMission.gameObject.SetActive(false);
            missionManager.gameObject.SetActive(true);
            mouseLook.enabled = true;
            playermovement.enabled = true;
            dInd = 0;
            return false;

        }
        panelDialougeBeforeMission.GetComponentInChildren<TextMeshProUGUI>().text = dialouges[dInd];

        dInd++;

        return dInd <= dialouges.Length;
    }
    public void NextIslandLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
