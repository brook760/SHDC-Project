using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> TypesOfEnemy;
    public int EnemyIndex;
    public int EnemyCount;

    public void EnemyDrop()
    {
        for(int i = 0; i < EnemyCount; ++i)
        {
            Instantiate(TypesOfEnemy[EnemyIndex],transform.localPosition, Quaternion.identity);
        }
           
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnemyDrop();
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
