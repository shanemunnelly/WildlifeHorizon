using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menuloader : MonoBehaviour
{
    [SerializeField] Transform loadingScreen;
    [SerializeField] Transform menu;

    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loadingScreen.gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
        }
    }
}
