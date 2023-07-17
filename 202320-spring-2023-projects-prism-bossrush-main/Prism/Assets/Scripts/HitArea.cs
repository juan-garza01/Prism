using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    private CapsuleCollider2D col;
    public HealthBar hp;
    private GameObject player, child, yPool, healthPool;
    private Transform parent;
    private ParticleSystem ps;
    private Vector3 location;
    public Coroutine coroutineLightning;
    private Transform ammo, heart;
    public float randomNum;
    private int rIntNum, choice;
    
    // Start is called before the first frame update
    void Awake()
    {
        yPool = GameObject.Find("YellowEnergyPool");
        healthPool = GameObject.Find("HealthPool");
        player = GameObject.FindGameObjectWithTag("Player");
        hp = player.GetComponent<HealthBar>();
        col = this.GetComponent<CapsuleCollider2D>();
        child = transform.GetChild(0).gameObject;
        parent = transform.parent;
    }

    public void Restart(ParticleSystem p){
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        ps = p;
        ps.Play();
        transform.parent = null;
        transform.position = player.transform.position;
        coroutineLightning = StartCoroutine(LightningWait());
    }

    IEnumerator LightningWait()
    {
        yield return new WaitForSeconds(.75f);
        col.enabled = true;
        child.SetActive(true);
        yield return new WaitForSeconds(1);
        col.enabled = false;
        child.SetActive(false);
        yield return new WaitForSeconds(.5f);
        yield return null;
        stopParticle();
        Repool();
    }

    private void stopParticle(){
        if (ps != null) ps.Stop();
    }

    private void Repool(){
        randomNum = Random.Range(0, 100) * Time.deltaTime * 100;
        rIntNum = (int)randomNum%10;
        

        if(rIntNum == 1 || rIntNum == 4){
            ammo = yPool.transform.GetChild(0);
            choice = 0;
        }
        else if( rIntNum == 7){
            heart = healthPool.transform.GetChild(0);
            choice = 1;   
        }
        else{
            choice = 2;
        }

        if(choice == 0){
            ammo.parent = null;
            ammo.position = this.transform.position;
        }
        else if(choice == 1)
        {
            heart.parent = null;
            heart.position = this.transform.position;
        }
        else if (choice == 2){
            Debug.Log("Nothing");
        }

        if (parent.gameObject.activeSelf){
            transform.parent = parent;
            gameObject.SetActive(false);
        }
        else Destroy(gameObject);
        
    }



    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            col.enabled = false;
            hp.hit(10);
        }
    }
}
