using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LvlLoader : MonoBehaviour
{
    public GameObject Loadingscreen;
    public Slider loader;
    public TextMeshProUGUI prText;
   public void loadLevel(int sceneIndex)
    {
        StartCoroutine(loadAsychronously(sceneIndex));
    } 
    IEnumerator loadAsychronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        Loadingscreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/.9f);
            loader.value = progress;
            prText.text = progress * 100f + "%";

            yield return null;
        }
    }
    
}
