
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenBox : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;
    RectTransform rectTransform;
    [SerializeField] Transform missionBox;

    float interpolateTime = 0;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    private void Update()
    {
        if (IsHaveMission())
        {
            interpolateTime += Time.deltaTime;

        }
        else
        {
            interpolateTime -= Time.deltaTime;

        }
        interpolateTime = Mathf.Clamp(interpolateTime, 0, 1);
        rectTransform.position = new Vector3(animationCurve.Evaluate(interpolateTime), rectTransform.position.y, rectTransform.position.z);

    }
    bool IsHaveMission()
    {
        return missionBox.childCount > 1;
    }
}
