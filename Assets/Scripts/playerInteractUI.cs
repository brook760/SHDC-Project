using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteractUI : MonoBehaviour
{
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private PlayerMove player;

    private void Update()
    {
        if (player.Gate() != null)
        {
            show();
        }
        else
            Hide();
    }
    private void show()
    {
        container.SetActive(true);
    }
    private void Hide()
    {
        container.SetActive(false);
    }
}
