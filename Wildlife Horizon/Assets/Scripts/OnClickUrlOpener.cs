using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickUrlOpener : MonoBehaviour
{
    [SerializeField] string urlToOpen;
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OpenUrl);
    }
    void OpenUrl()
    {
        Application.OpenURL(urlToOpen);
    }
}
