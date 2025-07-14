using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    private AiEnemy enemy;
    public GameObject Loot;

    private void Start()
    {
        enemy = GetComponent<AiEnemy>();
    }
    private void Update()
    {
        if(enemy.health <= 0)
        {
            Loot.SetActive(true);
            
        }
        else
        {
            Loot.SetActive(false);
        }
    }
}
