using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectEnabler : MonoBehaviour
{
    [SerializeField] GameObject[] toEnable;
    [SerializeField] GameObject[] toDisable;
    [SerializeField] bool isOnClickAction = true;
    private Button btn;
    private void Start()
    {
        if (isOnClickAction)
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(GameObjectEvent);
        }
    }
    // if I am disabling, my task to disbale others also
    // private void OnDisable()
    // {
    //     DisbaleGameObjects();
    // }
    public void GameObjectEvent()
    {
        EnableGameObjects();
        DisbaleGameObjects();
    }
    public void EnableGameObjects()
    {
        foreach (var go in toEnable)
        {
            if (!go.activeSelf)
                go.SetActive(true);
        }
    }

    public void DisbaleGameObjects()
    {
        foreach (var go in toDisable)
        {
            if (go.activeSelf)
                go.SetActive(false);
        }
    }

}
