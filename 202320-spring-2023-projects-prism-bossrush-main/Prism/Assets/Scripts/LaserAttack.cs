using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    private float laserCooldown, laserHitTime;
    private bool laserStop;
    public bool stop;
    private AttackController attackMove;
    private CrystalHealth crystalHP;
    //public List<LaserBeam> lasers = new List<LaserBeam>();
    // Start is called before the first frame update
    void Start()
    {
        Restart();
        //attackMove = transform.parent.parent.GetComponent<AttackController>();
    }
    public void Restart(){
        stop = false;
        laserStop = true;
        laserCooldown = Random.Range(4.0f, 7.0f);
        laserHitTime = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(laserStop){
            laserCooldown -= Time.deltaTime;
            if(laserCooldown<= 0){
                LaserShoot();
            }
        }else
        {
            laserHitTime -= Time.deltaTime;
            if(laserHitTime <= 0){
                LaserShootStop();
            }
        }
    }

    public void LaserShoot(){
        //if(attackMove != null) attackMove.StopMovement();
        stop = false;
        laserStop = false;
        laserCooldown = Random.Range(4.0f, 7.0f);
        foreach (Transform child in transform)
        {
            if(!child) continue;
            crystalHP = child.GetComponent<CrystalHealth>();
            crystalHP.enabled = false;
            if(crystalHP.health <= 0) continue;
            if(child.gameObject.activeSelf){
                child.GetChild(0).GetComponent<LaserBeam>().Shoot();
            }
        }
    }

    public void LaserShootStop(){
        laserStop = true;
        laserHitTime = Random.Range(5f, 10f);
        foreach (Transform child in transform)
        {
            child.GetChild(0).GetComponent<LaserBeam>().StopShooting();
            child.GetComponent<CrystalHealth>().enabled = true;
        }
        //if(attackMove != null) attackMove.StartMovement();
        stop = true;
    }
}
