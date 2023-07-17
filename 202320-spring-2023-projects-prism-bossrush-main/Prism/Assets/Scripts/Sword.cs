using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Create a list to hold all enemies that enter the area of sword
    private List <GameObject> objectsInTrigger = new List<GameObject>();
    private PolygonCollider2D polyCol;
    private bool hit = false; //only hits once not infinite times
    private float timer = 0f;
    private float cooldown = 0.5f;
    private bool swing = false;
    private bool redHit = false;
    private float swingTime = 1.5f;
    private EnemyStats enemy;
    private ColorStats cStats;
    public Animator animator;
    [SerializeField] private AudioSource SwordHit;
    [SerializeField] private AudioSource SwordMiss;
    private CrystalHealth cHealth;

    public void Start()
    {
        cStats = GameObject.Find("Canvas").GetComponent<ColorStats>();
        polyCol = gameObject.GetComponent<PolygonCollider2D>();
    }

    public void Update()
    {
        if(timer < cooldown)
        {
            timer += Time.deltaTime;
            //animation can go in here
        }
        else if (timer > swingTime)
        {
            gameObject.tag = "Sword";
            swing = false;
            timer = 0f;
        }
        else
        {
            if(cStats.red > 0)
            {
                animator.SetBool("Color",true);
                redHit = true;
            }
            else
            {
                animator.SetBool("Color", false);
                redHit = false;
            }

            if (Input.GetMouseButtonDown(0) && !swing)
            {
                gameObject.tag = "Weapon";
                cStats.lessAmmo();
                Debug.Log("Player Attack Sword");
                hit = true;
                swing = true;
                animator.SetTrigger("Attack");     
                

                foreach (GameObject obj in objectsInTrigger)
                {
                    if (obj != null && obj.activeSelf)
                    {
                        
                        if (obj.tag == "Enemy")
                        {
                            Debug.Log("HIT");
                            enemy = obj.gameObject.GetComponent<EnemyStats>();
                            enemy.enemyDamage(10, 0, redHit);
                            hit = false;
                            SwordHit.Play();
                        }
                        if(obj.tag == "Boss"){
                            cHealth = obj.gameObject.GetComponent<CrystalHealth>();
                            if(cHealth.enabled){
                                cHealth.HitBoss(0, redHit);
                            }
                        }
                    }
                    else
                    {
                        objectsInTrigger.Remove(obj);
                    }
                }
                gameObject.tag = "Weapon";
                SwordMiss.Play();
            }
            else
            {
                StartCoroutine(wait());
                timer += Time.deltaTime;               
            }
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
        yield return new WaitForSeconds(1f);
        //gameObject.tag = "Sword";
    }


}