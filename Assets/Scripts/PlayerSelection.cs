using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] player;
    public int playerIndex;
    public GameManager gameManager;
    private readonly List<string> animationList = new()
    { "Idle","Walking","Running",};
    [Space(10)]
    Transform animal_parent;
    Dropdown dropdownValue;
    Dropdown dropdownAnimation;
    public void GameRun()
    {
        PlayerPrefs.SetInt("SelectedID",gameManager.playerIndex);
    }
    private void Update()
    {
        gameManager.playerIndex = playerIndex;

    }
    void Start()
    {
        animal_parent = GameObject.FindWithTag("Player").transform;

        Transform canvas = GameObject.Find("Canvas").transform;

        dropdownValue = canvas.Find("PlayerUI").Find("Dropdown").GetComponent<Dropdown>();
        dropdownAnimation = canvas.Find("Animation").Find("Dropdown").GetComponent<Dropdown>();


        int count = animal_parent.childCount;
        player = new GameObject[count];
        List<string> animalList = new();

        for (int i = 0; i < count; i++)
        {
            player[i] = animal_parent.GetChild(i).gameObject;
            string n = animal_parent.GetChild(i).name;
            animalList.Add(n);
            // animalList.Add(n.Substring(0, n.IndexOf("_")));

            if (i == 0) player[i].SetActive(true);
            else player[i].SetActive(false);
        }

        dropdownValue.AddOptions(animalList);
        dropdownAnimation.AddOptions(animationList);
    }
    public void Next()
    {
        if (dropdownValue.value >= dropdownValue.options.Count - 1)
            dropdownValue.value = 0;
        else
            dropdownValue.value++;

        ChangePlayer();
    }

    public void Previous()
    {
        if (dropdownValue.value <= 0)
            dropdownValue.value = dropdownValue.options.Count - 1;
        else
            dropdownValue.value--;

        ChangePlayer();
    }

    public void ChangePlayer()
    {
        player[playerIndex].SetActive(false);
        player[dropdownValue.value].SetActive(true);
        playerIndex = dropdownValue.value;

        ChangeAnimation();
    }

    public void NextAnimation()
    {
        if (dropdownAnimation.value >= dropdownAnimation.options.Count - 1)
            dropdownAnimation.value = 0;
        else
            dropdownAnimation.value++;

        ChangeAnimation();
    }


    public void PrevAnimation()
    {
        if (dropdownAnimation.value <= 0)
            dropdownAnimation.value = dropdownAnimation.options.Count - 1;
        else
            dropdownAnimation.value--;

        ChangeAnimation();
    }

    public void ChangeAnimation()
    {
        if (player[dropdownValue.value].TryGetComponent<Animator>(out var animator))
        {
            int index = dropdownAnimation.value;

            // If Spin/Splash animation
            if (index == 15)
            {
                if (animator.HasState(0, Animator.StringToHash("Spin")))
                {
                    animator.Play("Spin");
                    // dropdownAnimation.options[index] = new Dropdown.OptionData("Spin");
                }
                else if (animator.HasState(0, Animator.StringToHash("Splash")))
                {
                    animator.Play("Splash");
                    // dropdownAnimation.options[index] = new Dropdown.OptionData("Splash");
                }
            }
            else
            {
                animator.Play(dropdownAnimation.options[index].text);
            }
        }
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
