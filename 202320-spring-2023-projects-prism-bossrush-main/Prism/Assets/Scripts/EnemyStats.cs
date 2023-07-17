using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{
    private int health, maxhealth, rIntNum, choice, chCount, originalCh;
    private bool onhit = false;
    public bool onsight, dontmove, idle, attackPlayer, selfDestructed;
    public float randomNum, count, counthit, cooldown = 2, timer;
    private AIPath aiPath;
    private AIDestinationSetter aiDest;
    private HealthBar healthbar;
    public Animator animator;
    public BoxCollider2D hitbox;
    private Vector3 distance, lastLocation;
    private GameObject player, rPool, bPool, yPool, healthPool;
    private Transform ammo, heart;
    private Rigidbody2D enemy;

    [SerializeField] private AudioSource movingSound;
    [SerializeField] private AudioSource hitMarker;
    [SerializeField] private AudioSource deathSound;

    public int enemyID;

    private GameManagerScript gameScript;

    // Start is called before the first frame update
    void Start()
    {
        gameScript = GameObject.Find("GameManager").transform.GetComponent<GameManagerScript>();
        gameScript.EnemySpawn();
        //enemy = GetComponent<Rigidbody2D>();
        attackPlayer = true;
        player = GameObject.FindGameObjectWithTag("Player");
        selfDestructed = false;
        if (enemyID >= 0 && enemyID <= 2){
            maxhealth = 100;
        }
        dontmove = false;
        
        
        healthbar = player.GetComponent<HealthBar>();
        timer = count = counthit = 0f;
        health = maxhealth;
        if(enemyID == 0 || enemyID == 1){
            aiPath = transform.parent.parent.gameObject.GetComponent<AIPath>();
            aiDest = transform.parent.parent.gameObject.GetComponent<AIDestinationSetter>();
            aiDest.target = player.transform;
        }
        //locationOld = locationNew = transform.position;
        distance = new Vector3(0,0,0);
        idle = true;

        rPool = GameObject.Find("RedEnergyPool");
        bPool = GameObject.Find("BlueEnergyPool");
        yPool = GameObject.Find("YellowEnergyPool");
        healthPool = GameObject.Find("HealthPool");
        hitbox = this.GetComponent<BoxCollider2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;

        
        if(health <= 0){
            if(enemyID == 0 || enemyID == 1){
                aiPath.canMove = false;
                movingSound.Pause();
            }
            if(!deathSound.isPlaying) {
                deathSound.Play();
            }

            hitbox.enabled = false;

            onhit = false;
            
            animator.SetBool("Death", true);
            count += Time.deltaTime;
            if (count >= .66f){
                count = 0f;
                repoolArrows();
                repool();
                animator.enabled = false;

            }
        }
        else{

            if(enemyID == 0 || enemyID == 1)
                distance = transform.position - lastLocation;
                
            else if(enemyID == 2)
                distance = player.transform.position - transform.position;


            animator.SetFloat("Y", distance.y);
            animator.SetFloat("X", distance.x);            
            animator.SetBool("Idle", idle);

            
            if(enemyID == 0 || enemyID == 1){
                    if(aiPath.canMove){
                        idle = false;
                        if(!movingSound.isPlaying) {
                        movingSound.Play();
                    }
                    else{
                        idle = true;
                    }
                }
                lastLocation = transform.position;
            }

           
            if(dontmove){
                if(enemyID == 0 || enemyID == 1){
                    if(player.transform.position.y <= transform.position.y){
                        animator.SetFloat("LastY", -1);
                    }
                    else{
                        animator.SetFloat("LastY", 1);
                    }
                     
                }
                else if(enemyID == 2){
                    animator.SetFloat("LastY", distance.y);
                    animator.SetFloat("LastX", distance.x);
                }
                
            }

        if(onhit){
            animator.SetBool("Hit", true);
            count += Time.deltaTime;
            if (count >= .33f){
                count = 0f;
                onhit = false;
                animator.SetBool("Hit", false);
            }
        }

        }
    }

    public void enemyDamage(int hit, int color, bool extra)
    {
        if (color == enemyID)
        {
            if(extra)
                hit *= 5;
            else{
                hit *= 2;
            }
        }
        health -= hit;
        onhit = true;
        hitMarker.Play();
        Debug.Log("Enemy Hit with " + hit);
    }

    public void selfDestruct() {
        selfDestructed = true;
        health = 0;
        healthbar.hit(25);
        Debug.Log("Enemy self destructed");
    }

    private void repool()
    {
        switch(enemyID){
            case 0:
                Drop(rPool);
                break;
            case 1:
                Drop(bPool);
                break;
            case 2:
                Drop(yPool);
                break;
        }

        /*
        if(enemyID == 2)
            transform.parent.localPosition = new Vector3(0, 0, 0);
        else if(enemyID == 0){
            transform.parent.parent.localPosition = new Vector3(0, 0, 0);
            aiPath.maxSpeed = 0;
        }
        */
        gameScript.LessEnemy();
        if(enemyID == 0 || enemyID == 1)
            Destroy(transform.parent.parent.gameObject);
        else if(enemyID == 2)
            Destroy(transform.parent.gameObject);

        animator.SetBool("Death", false);
        health = maxhealth;
        
        this.enabled = false;

    }

    private void repoolArrows(){
        if(enemyID == 0 || enemyID == 1){
            originalCh = 0;
        }
        else if(enemyID == 2){
            originalCh = 1;
        }

        chCount = transform.childCount;
        for (; chCount > originalCh; chCount--){
            transform.GetChild(chCount-1).parent = null;
        }

    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player" && attackPlayer &&(distance.x < 0.01 && distance.x > -0.01 && distance.y < 0.01 && distance.y > -0.01) &&(enemyID == 0 || enemyID == 2)){
            healthbar.hit(4);
            attackPlayer = false;
            StartCoroutine(CooldownHit());
        }         
    }

    private void Drop(GameObject pool){
        randomNum = Random.Range(0, 100) * Time.deltaTime * 100;
        rIntNum = (int)randomNum%10;
        

        if(rIntNum%3 == 0){
            choice = 0;
            ammo = pool.transform.GetChild(0);
        }
        else if(rIntNum%2 == 0){
            heart = healthPool.transform.GetChild(0);
            //red thing
            choice = 1;   
        }
        else{
            //health thing
            choice = 2;
        }

        if(choice == 0){
            Debug.Log("Color");
            ammo.parent = null;
            ammo.position = this.transform.position;
        }
        else if(choice == 1)
        {
            Debug.Log("Health");
            heart.parent = null;
            heart.position = this.transform.position;
        }
        else if (choice == 2){
            Debug.Log("Nothing");
        }

        Debug.Log("Choice" + rIntNum);
        

    }

    IEnumerator CooldownHit()
    {
        yield return new WaitForSeconds(.25f);
        attackPlayer = true;
        
    }

}