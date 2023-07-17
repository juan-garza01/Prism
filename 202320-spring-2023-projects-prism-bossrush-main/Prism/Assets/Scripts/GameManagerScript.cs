using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public bool isPaused;
    public GameObject gameOverUI;
    private GameObject winUI;
    private int enemiesAmount = 0;
    private bool spawnEnemy = false;
    public Animator cFade;
    public float fadeTime = 1f;
    public TextMeshProUGUI enemyLeft;
    private string enemyPart;
    private int sceneNum = 0;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
        enemyPart = enemyLeft.text;
        enemyLeft.text = "";
        isPaused = false;
        winUI = GameObject.Find("Win");
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnEnemy && enemiesAmount == 0){
            Debug.Log("THERE ARE " + enemiesAmount + " ENEMIES");
            PlayGame();
        }
        /*
       if(gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }*/
    }

    public void gameOver()
    {
        isPaused = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Win(){
        StartCoroutine(WinWait());
    }

    public void End(){
        winUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
    public void restart()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }
    public void quit()
    {
        isPaused = false;
        Application.Quit();
    }

    public void EnemySpawn(){
        enemiesAmount++;
        enemyLeft.text = enemyPart + enemiesAmount.ToString();
        spawnEnemy = true;
    }

    public void LessEnemy(){
        enemiesAmount--;
        enemyLeft.text = enemyPart + enemiesAmount.ToString();
        Debug.Log("THERE ARE " + enemiesAmount + " ENEMIES");
    }

    public void PlayGame()
    {
        isPaused = false;
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator Load(int lvlIndex){
        cFade.SetTrigger("Start");
        yield return new WaitForSeconds(fadeTime);
        isPaused = false;
        SceneManager.LoadScene(lvlIndex);
    }

    IEnumerator WinWait(){
        yield return new WaitForSeconds(2);
        End();
    }
}

