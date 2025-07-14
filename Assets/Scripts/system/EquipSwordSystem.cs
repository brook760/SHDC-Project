using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSwordSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponSheath;
    public AudioSource source;
    public AudioClip SwordSound;

    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;
    void Start()
    {
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
    }
    public void DrawWeapon()
    {
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponInSheath);
        int count = weaponSheath.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            Destroy(weaponSheath.transform.GetChild(i).gameObject);
        }
    }

    public void SheathWeapon()
    { 
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);
        int count = weaponHolder.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(weaponHolder.transform.GetChild(i).gameObject);
        }
    }
    public void StartDealDamage()
    {
        source.PlayOneShot(SwordSound,1);
        currentWeaponInHand.GetComponentInChildren<DamageSystem>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageSystem>().EndDealDamage();
    }
}
