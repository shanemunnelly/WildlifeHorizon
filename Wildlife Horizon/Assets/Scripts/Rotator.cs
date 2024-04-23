using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] bool isX, isY, isZ;
    private int x, y, z;
    private void Update()
    {
        if (isX) x = 1;
        if (isY) y = 1;
        if (isZ) z = 1;
        this.transform.Rotate(new Vector3(x, y, z) * speed * Time.deltaTime);
    }
}
