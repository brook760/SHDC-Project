using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWidget : MonoBehaviour
{
    public TMPro.TMP_Text ammoText;
    public void Referesh(int ammoCount)
    {
        ammoText.text = ammoCount.ToString();
    }
}
