using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    public int itemNum = 0;
    private GameObject sw, sh, bw;
    private ColorStats stats;
    private HealthBar health;
    private Transform myparent;
    [SerializeField] private AudioSource collectionSound;

     void Start()
    {
        myparent = this.transform.parent;
        sw = GameObject.Find("sword");
        sh = GameObject.Find("shield");
        bw = GameObject.Find("bow");

        stats = GameObject.Find("Canvas").GetComponent<ColorStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            collectionSound.Play();
            switch(itemNum){
                case 1:
                    stats.addRed(10);
                    Destroy(gameObject);
                    break;
                case 2:
                    stats.addBlue(10);
                    Destroy(gameObject);
                    break;
                case 3:
                    stats.addYellow(10);
                    Destroy(gameObject);
                    break;
                case 4:
                    stats.addRed(4);
                    reparent();
                    break;
                case 5:
                    stats.addBlue(4);
                    reparent();
                    break;
                case 6:
                    stats.addYellow(4);
                    reparent();
                    break;
                case 7:
                    health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>();
                    health.heal(8);
                    reparent();
                    break;
                case 8:
                    break;
            }
            

        }
    }

    private void reparent(){
        transform.parent = myparent;
        transform.localPosition = new Vector3(0,0,0);
    }
}
