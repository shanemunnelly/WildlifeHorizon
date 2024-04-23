using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private Transform journalBook;
    [SerializeField] private Animator animator;
    [SerializeField] Transform canvasMission;
    [SerializeField] Transform page0;
    bool isMissionActive;
    private int hash_OpenJournal = Animator.StringToHash("OpenJournal");
    private int hash_CloseJournal = Animator.StringToHash("CloseJournal");
    bool isOpened = false;
    public static List<Transform> unlocledPages = new List<Transform>();
    int currInd = 0;
    private void Start()
    {
        unlocledPages.Add(page0);
    }
    public void OpenJournal()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        journalBook.gameObject.SetActive(true);
        animator.Play(hash_OpenJournal);
        isMissionActive = canvasMission.gameObject.activeSelf;
        canvasMission.gameObject.SetActive(false);
        if (unlocledPages.Count > 0)
            unlocledPages[currInd].gameObject.SetActive(true);

        isOpened = true;
    }
    public void CloseJournal()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator.Play(hash_CloseJournal);
        Invoke("CloseJournalAfterAnimationDone", 1f);
    }

    void CloseJournalAfterAnimationDone()
    {
        journalBook.gameObject.SetActive(false);
        canvasMission.gameObject.SetActive(isMissionActive);
        isOpened = false;

    }

    private void Update()
    {
        if (!isOpened && Input.GetKeyDown(KeyCode.J))
        {
            OpenJournal();
        }
        else if (isOpened && Input.GetKeyDown(KeyCode.J))
        {
            CloseJournal();
        }
    }
    public void NextPage()
    {
        if (unlocledPages.Count == 0) return;

        unlocledPages[currInd].gameObject.SetActive(false);
        currInd = (currInd + 1) % unlocledPages.Count;
        unlocledPages[currInd].gameObject.SetActive(true);

    }
    public void PrevPage()
    {
        if (unlocledPages.Count == 0) return;

        unlocledPages[currInd].gameObject.SetActive(false);
        currInd = (currInd + unlocledPages.Count - 1) % unlocledPages.Count;
        unlocledPages[currInd].gameObject.SetActive(true);

    }
}
