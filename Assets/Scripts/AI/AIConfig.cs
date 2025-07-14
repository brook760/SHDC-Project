using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIConfig : ScriptableObject
{
    public int health = 50;
    public float AttackCD = 3f;
    public float AttackRange = 1.2f;
    public float LookRange = 4f;
}
