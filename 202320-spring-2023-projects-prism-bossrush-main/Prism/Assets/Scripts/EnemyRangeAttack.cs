using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{
    private Camera cam;
    private Vector3 playerlocation, rotation;
    public GameObject arrow, player;
    public Transform slimeBullet;
    public bool canShoot, canHit, once;
    public LayerMask hitAreas;
    private float cooldown, formingTime, timer, rotateZ;
    public GameObject lastHit;
    public Animator animator;
    public Vector3 coll, location;
    private SlimeBallBullet bullet;
    private EnemyStats es;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        cooldown = 1f;
        formingTime = .5f;
        player = GameObject.FindGameObjectWithTag("Player");
        slimeBullet = gameObject.transform.GetChild(0);
        animator = slimeBullet.GetComponent<Animator>();
        es = this.transform.parent.GetComponent<EnemyStats>();

        canHit = false;
        canShoot = true;
        once = true;
    }

    // Update is called once per frame
    void Update()
    {
        //var ray = new Ray(transform.position, this.transform.forward);
        rotation = player.transform.position - transform.position;
        rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(rotation.x, rotation.y+1), 7f, hitAreas);
        //Debug.DrawRay(transform.position, new Vector2(rotation.x, rotation.y+1), Color.green,0, true);
        //lastHit = hit.transform.gameObject;
        //coll = hit.point;
        

        if(!canShoot){
            timer += Time.deltaTime;
            if(timer > cooldown)
            {
                canShoot = true;
                timer = 0;
            }
        }
        else{
            //animate first then cancel let cooldown shoot
            
            if(hit && hit.transform.gameObject == player){
                es.dontmove = true;
                canHit = true;
            }
            else{
                es.dontmove = false;
            }
                
            if(canHit){
                if(once){

                    if(transform.childCount > 0)
                    {
                        slimeBullet = gameObject.transform.GetChild(0);
                        animator = slimeBullet.GetComponent<Animator>();
                        slimeBullet.gameObject.SetActive(true);
                        once = false;
                    }
                }
                if(!once){
                    timer += Time.deltaTime;
                }
            }

            if(timer >= formingTime && hit && hit.transform.gameObject == player){
                
                animator.SetBool("Shoot", true);
                slimeBullet.GetComponent<SlimeBallBullet>().enabled = true;
                canShoot = canHit = false;
                timer = 0;
                once = true;
                
            }

            
        }
    }
}
