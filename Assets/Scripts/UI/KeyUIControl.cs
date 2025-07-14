using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIControl : MonoBehaviour
{
    public GameObject[] keys;
    public bool UIOn = false;
    void Start()
    {
        int count = transform.childCount;
        keys = new GameObject[count];
        List<string> KeysObtained = new();
        for (int i = 0; i < count; i++)
        {
            keys[i] = transform.GetChild(i).gameObject;
            string n = transform.GetChild(i).name;
            KeysObtained.Add(n);
        }
    }
    public void KeyManage()
    {
        for (int i = 0; i < 11; i++)
        {
            if (UIOn)
            {
                keys[i].GetComponent<Image>().enabled = true;
                keys[i].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            }
            if (!UIOn)
            {
                keys[i].GetComponent<Image>().enabled = false;
                keys[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }

        }
    }
    void Update()
    {
        KeyManage();
        if(Input.GetKey(KeyCode.T))
        {
            UIOn = !UIOn;

        }
    }
}
