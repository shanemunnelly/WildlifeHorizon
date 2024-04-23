using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class OnEnableAnimationPlayer : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string animationToPlay = "NextPage";
    TextMeshProUGUI text;
    Image img;
    Color textColor;
    Color imgColor;
    float alphaChannel = 0;
    bool isPageOpen;
    private void Awake()
    {
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        img = this.GetComponentInChildren<Image>();
        textColor = text.color;
        imgColor = img.color;
    }
    private void OnEnable()
    {
        animator.Play(animationToPlay);

        isPageOpen = false;
        alphaChannel = 0;

        text.color = new Color(textColor.r, textColor.g, textColor.b, alphaChannel);

        img.color = new Color(imgColor.r, imgColor.g, imgColor.b, alphaChannel);
        Invoke("ShowPage", 0.9f);
    }
    void ShowPage()
    {

        isPageOpen = true;

    }

    private void Update()
    {
        if (!isPageOpen) return;
        alphaChannel = Mathf.Lerp(alphaChannel, 1, Time.deltaTime);
        text.color = new Color(textColor.r, textColor.g, textColor.b, alphaChannel);
        img.color = new Color(imgColor.r, imgColor.g, imgColor.b, alphaChannel);


    }
}
