using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator cFade;
    public float fadeTime = 1f;
    public void PlayGame()
    {
        Debug.Log("Start has been pressed");
        //SceneManager.LoadScene("DEMO MAP BATTLE")
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void QuitGame()
    {
        Debug.Log("Quit has been pressed");
        Application.Quit();
    }

    IEnumerator Load(int lvlIndex){
        cFade.SetTrigger("Start");
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(lvlIndex);
    }
}
