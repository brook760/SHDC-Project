using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPopUpObject : MonoBehaviour
{
    public LockedObject locked;
    public GameObject SpawnObject;

    private void Update()
    {
        if (locked.isopen)
        {
            SpawnObject.SetActive(true);
        }
        else
        {
            SpawnObject.SetActive(false);
        }
    }
}
