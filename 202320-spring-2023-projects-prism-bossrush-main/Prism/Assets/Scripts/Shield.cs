using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool canAttck = false;
    // Create a list to hold all enemies that enter the area of shield
    private List <GameObject> objectsInTrigger = new List<GameObject>();
    // Start is called before the first frame update
    private bool shNoise = false;
    private CircleCollider2D circleCol;
    private HealthBar HealthBar;
    [SerializeField] private AudioSource shieldActive;
    public SpriteRenderer SR;
    private Color grey, blue;
    private bool color, noColor, shieldOn;
    private ColorStats cStats;
    private CapsuleCollider2D playerCol;
    //looks for enemystats file
    private EnemyStats enemy;
    private GameObject player;

    private float counter = 1f;
    
    private CrystalHealth cHealth;
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        blue = new Color(0, 14f/255f, 1f, 150f/255f);
        grey = new Color(175f/255f, 175f/255f, 175f/255f, 50f/255f);
        circleCol = gameObject.GetComponent<CircleCollider2D>();
        circleCol.enabled = false;
        cStats = GameObject.Find("Canvas").GetComponent<ColorStats>();
        HealthBar = player.GetComponent<HealthBar>();
        playerCol = player.GetComponent<CapsuleCollider2D>();
        SR = gameObject.GetComponent<SpriteRenderer>();
        SR.enabled = false;
        noColor = true;
        color = false;
        shieldOn = false;
        
        if(cStats.blue > 0){
            noColor = false;
            color = true;
        }
        else{
            noColor = true;
            color = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(noColor && cStats.blue == 0 ){
            SR.color = grey;
            HealthBar.color = false;
            noColor = false;
            color = true;
            canAttck = false;
        }
        else if(color && cStats.blue > 0){
            SR.color = blue;
            HealthBar.color = true;
            noColor = true;
            color = false;
            canAttck = true;
        }
        
        if (Input.GetMouseButton(0))
        {
            enableShield();
        }
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && canAttck)
        {   
            //Shield yourself
            enableShield();
            //Damage the enemy!
            if (cStats.blue > 0)
            {
                gameObject.tag = "Weapon";
                foreach (GameObject obj in objectsInTrigger)
                {
                    if (obj != null && obj.activeSelf)
                    {
                        if (obj.tag == "Enemy")
                        {
                                Debug.Log("Shield Hit");
                                cStats.lessAmmo();
                                enemy = obj.gameObject.GetComponent<EnemyStats>();
                                enemy.enemyDamage(10,1,true);
                                
                        }
                        if(obj.tag == "Boss"){
                            cHealth = obj.gameObject.GetComponent<CrystalHealth>();
                            if(cHealth.enabled){
                                cHealth.HitBoss(1, true);
                            }
                        }
                    }
                    else
                    {
                        objectsInTrigger.Remove(obj);
                    }
                }
                canAttck = false;
                StartCoroutine(wait());
            }
            else
            {
                gameObject.tag = "Player";
            }

        }
        if (Input.GetMouseButtonUp(1)){
            gameObject.tag = "Player";
        }
        if(Input.GetMouseButtonUp(0))
        {
            gameObject.tag = "Player";
            Debug.Log("UP");
            disableShield();
        }
    }
    public void enableShield(){
        repoolChildren();
        circleCol.enabled = true;
        HealthBar.shield = true;
        SR.enabled = true;
        shieldOn = true;
        //player.tag = "Weapon";
        //gameObject.tag = "Weapon";
        shieldActive.Play();
        playerCol.enabled = false;
    }
    public void disableShield(){
            repoolChildren();
            circleCol.enabled = false;
            HealthBar.shield = false;
            SR.enabled = false;
            shieldOn = false;
            //player.tag = "Player";
            gameObject.tag = "Shield";
            playerCol.enabled = true;
            shieldActive.Pause();
    }

    private void repoolChildren(){
        gameObject.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in transform)
        {
            child.GetComponent<SlimeBallBullet>().Repool();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!objectsInTrigger.Contains(other.gameObject))
        {
            objectsInTrigger.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsInTrigger.Contains(other.gameObject))
        {
            objectsInTrigger.Remove(other.gameObject);
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.tag = "Player";
        yield return new WaitForSeconds(0.5f);
        canAttck = true;
    }
}
