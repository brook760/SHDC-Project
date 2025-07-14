using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public float time = 4f;
    public float speed = 15;
    public GameObject scene;
    public int spin;
    public List<GameObject> list;

    private void Update()
    {
        if(time >= 0)
        {
            scene.transform.Rotate(0,speed*Time.deltaTime, 0);
            time -= Time.deltaTime;
        }
        if(spin == 0)
        {
            list[0].SetActive(true);
        }
        if (spin == 1 || spin ==-1)
        {
            list[0].SetActive(false);
            list[1].SetActive(true);
        }
        else if(spin == 2 || spin ==-2)
        {
            list[1].SetActive(false);
            list[2].SetActive(true);
        }
    }
    public void left()
    {
        spin += 1;
        time += 4f;
        speed = +15f; 
    }
    public void right()
    {
        spin -= 1;
        time += 4f;
        speed = -15f;
    }
}
