using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public FlashDMG fd;
    private Slider slider;
    private float currHealth;
    public float timer;
    public bool shield = false;
    public int health = 100;
    private int healthState;
    private bool damage, healUp;
    public GameObject h;
    public Image heart;
    public bool color = false;
    public Sprite[] heartArray;
    public GameManagerScript gameManager;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        h = GameObject.Find("HealthBar");
        slider = h.GetComponent<Slider>();
        slider.maxValue = health;
        slider.value = health;
        heart = h.transform.GetChild(2).GetComponent<Image>();
        healthState = 0;
        damage = healUp = false;
        isDead = false;
        fd = gameObject.GetComponent<FlashDMG>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

    }

    void Update(){
        if(damage  || healUp){
            timer += Time.deltaTime;

            if(timer >= 0.10f){
                switch(healthState){
                    case 0:
                        heart.sprite = heartArray[0];
                        break;
                    case 1:
                        heart.sprite = heartArray[1];
                        break;
                    case 2:
                        heart.sprite = heartArray[2];
                        break;
                }
                damage = healUp = false;
                timer = 0;
            
            }
            if (health <= 0 && !isDead){
                //Invoke("QuitGame", 1f);
                isDead = true;
                
                gameManager.gameOver();
                //Debug.Log("Dead");
            }
        }
    }

    private void QuitGame(){
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void hit(int lose)
    {
        if (!shield || !color)
        {
            fd.Flash();
            damage = true;
            heart.sprite = heartArray[3];

            if(!color && shield)
                lose /= 2;
            Debug.Log(lose);

            health -= lose;
            
            if (health<=0)
            {
                if(healthState <= 1 && health <= 0){
                    healthState = 2;
                }
                health = 0;
                slider.value = 0;
            }
            else{
                if(healthState == 0 && health <= 50){
                    healthState = 1;
                }
                slider.value = health;
            }
        }
    }

    public void heal(int gain)
    {
        if (health < 100)
        {
            
            heart.sprite = heartArray[3];
            healUp = true;
            health += gain;
            if (healthState == 1 && health > 50){
                    healthState = 0;
            }
            slider.value = health;
        }
    }
}