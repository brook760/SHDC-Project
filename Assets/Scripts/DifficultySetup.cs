using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetup : MonoBehaviour
{
    public AIConfig[] config;
    void Update()
    {
        
    }
    public void Normal()
    {
        config[0].health = 40;
        config[0].AttackCD = 3f;
        config[1].health = 80;
        config[1].AttackCD = 5f;
        config[2].health = 20;
        config[2].AttackCD = 1f;
        config[3].health = 120;
        config[3].AttackCD = 10f;
    }
    public void Hard()
    {
        config[0].health = 80;
        config[0].AttackCD = 1.5f;
        config[1].health = 160;
        config[1].AttackCD = 2.5f;
        config[2].health = 40;
        config[2].AttackCD = .5f;
        config[3].health = 240;
        config[3].AttackCD = 5f;
    }
}
