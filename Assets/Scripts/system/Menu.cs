using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [Header("First Selceted Button")]
    [SerializeField] private GameObject firstSelected;

    protected virtual void OnEnable()
    {
        StartCoroutine(SetFirstSelected(firstSelected));
    }

    public IEnumerator SetFirstSelected(GameObject first)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(first);
    }
}
