using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyer : MonoBehaviour
{
    [SerializeField] GameManager gm;

    public int destroyedCar = 0;

    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        destroyedCar++;
        if (destroyedCar == gm.carSpawnCount[gm.question]) gm.SetPanel();
    }
}
