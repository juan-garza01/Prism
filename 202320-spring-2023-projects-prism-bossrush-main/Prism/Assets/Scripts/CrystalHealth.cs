using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 6;
    private int state;
    public Animator aCrystal;
    private Transform child;
    private float count = 0f;
    private int orgChildAmount;
    private YellowBossHealth bossHP;
    private LaserBeam lb;
    private GameObject hitSpot;
    private HitArea hArea;

    [SerializeField] private AudioSource explodeSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource colorSound;
    [SerializeField] private AudioSource noColorSound;
    bool once = true;
    void Start()
    {
        orgChildAmount = transform.childCount;
        bossHP = gameObject.transform.parent.parent.GetComponent<YellowBossHealth>();
        lb = transform.GetChild(0).GetComponent<LaserBeam>();
        hitSpot = transform.GetChild(2).gameObject;
        hArea = hitSpot.GetComponent<HitArea>();
        Restart();
    }

    public void Restart(){
        gameObject.SetActive(true);
        aCrystal.Play("CrystalForming");
        state = 0;
        health = 6;
        aCrystal.SetInteger("Hit", state);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            
            gameObject.GetComponentInChildren<LaserBeam>().end = true;
            count += Time.deltaTime;
            if(once){
                once = false;
                explodeSound.Play();
                //aCrystal.SetInteger("Hit", 4);
            }
            if(count > .6){
                RepoolArrows();
                if(!explodeSound.isPlaying)
                {
                    count = 0;
                    if(hArea.coroutineLightning != null) hArea.StopCoroutine(hArea.coroutineLightning);
                    hitSpot.transform.rotation = Quaternion.identity;
                    hitSpot.transform.parent = transform;
                    bossHP.lessHealth();
                    bossHP.Respawn(this, lb);
                    gameObject.SetActive(false);   
                }
                
            }
            
        }
    }

    public void HitBoss(int colorID, bool power){
        if(aCrystal.GetCurrentAnimatorStateInfo(0).IsName("CrystalForming")) return;
        
        if(colorID == 2){
            if(power){
                noColorSound.Play();
                health -= 3;
                state += 3;
            }
            else{
                state += 2;
                health -= 2;
                colorSound.Play();
            }
            
        } 
        else{
            state++;
            health--;
            hitSound.Play();
        } 
        aCrystal.SetInteger("Hit", state);
        aCrystal.SetBool("onHit", true);
        StartCoroutine(WaitHit());
    }

    IEnumerator WaitHit(){
        yield return new WaitForSeconds(.01f);
        aCrystal.SetBool("onHit", false);

    }

    public void AnimationState(){
        if(state >= 6) aCrystal.SetBool("Explode", true);
        else if(state >= 4) aCrystal.SetBool("Reposition2", true);
        else if(state >= 2) aCrystal.SetBool("Reposition", true);
        aCrystal.SetInteger("Hit", state);
    }

    public void RepoolArrows(){
        for(int chCount = transform.childCount; chCount > orgChildAmount; chCount--){
            //if(child.tag != "Arrow") continue;
            child = transform.GetChild(chCount-1);
            child.transform.parent = null;
            if(child.tag != "Arrow") continue;
            child.GetComponent<ArrowScript>().Repool();
        }
        hitSpot.transform.parent = transform;
        hitSpot.SetActive(false);
    }
}
