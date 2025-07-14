using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Rigidbody rb;
    public Transform player;
    public Vector3 target, offset;
    public float SlideSpeed;
    bool picked;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player = other.gameObject.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Vector3 falseOffest = Vector3.zero;

                if (!picked)
                {
                    falseOffest = offset;
                    picked = true;
                }
                if (picked)
                {
                    falseOffest = Vector3.zero;
                    picked = false;
                }

                target = player.position + falseOffest;
                transform.position =
                    Vector3.Lerp(transform.position, target, SlideSpeed * Time.deltaTime);
            }
        }
    }
}
