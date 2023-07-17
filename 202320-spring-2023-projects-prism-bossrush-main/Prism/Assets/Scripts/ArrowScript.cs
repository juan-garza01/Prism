using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Vector3 mouse, dir, rotate;
    private Camera cam;
    private Rigidbody2D rb;
    private float force = 10f;
    private float r;
    private GameObject parent;
    private bool once, player, yellowHit;
    public LayerMask arrowHit;
    private ColorStats cstat;
    private BoxCollider2D arrowCollider;
    
    public Animator animate;
    private CrystalHealth cHealth;
    // Start is called before the first frame update
    void Awake()
    {
        once = true;
        yellowHit = false;
        parent = gameObject.transform.parent.gameObject;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        cstat = GameObject.Find("Canvas").GetComponent<ColorStats>();
        arrowCollider = this.GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(once){
            if(cstat.yellow > 0){
                animate.SetBool("Color", true);
                yellowHit = true;
                cstat.lessAmmo();
            }
            else{
                yellowHit = false;
            }
            
            once = false;
            arrowCollider.enabled = true;
            transform.parent = null;
            mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            dir = mouse - transform.position;
            rotate = transform.position - mouse;
            rb.velocity = new Vector2(dir.x, dir.y).normalized * force;
            r = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, r+90);
            
            Invoke("Repool", 4);
        }
        
    }

    public void Repool(){
        rb.velocity = new Vector2(0,0).normalized;
        transform.parent = parent.transform;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0,0,0);
        //
        once = true;
        this.enabled = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other){
        if ((arrowHit.value & (1 << other.transform.gameObject.layer)) > 0){
            animate.SetBool("Hit", true);
            rb.velocity = new Vector2(0,0).normalized;
            transform.parent = other.transform;
            arrowCollider.enabled = false;
        }
        if(other.tag == "Enemy"){
            
            Debug.Log("hit Enemy");
            other.gameObject.GetComponent<EnemyStats>().enemyDamage(10, 2, yellowHit);
            CancelInvoke("Repool");
            Invoke("Repool", 1);
        }
        if(other.tag == "Boss"){
            cHealth = other.GetComponent<CrystalHealth>();
            if(cHealth.enabled){
                cHealth.HitBoss(2, yellowHit);
            }
            
        }
    }
}
