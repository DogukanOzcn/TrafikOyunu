using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool moveable = false;

    float carSpeed = 0.05f;

    void FixedUpdate()
    {
        if(moveable) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + carSpeed);
    }
}
