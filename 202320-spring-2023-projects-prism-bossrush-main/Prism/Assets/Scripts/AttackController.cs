using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
public class AttackController : MonoBehaviour
{
    private LaserAttack attack1;
    private LightningAttack attack2;
    private MovementCorrector move1;
    private MovementInfinityPath moveIP;

    private int attack = 2;
    private int moveState = 1;
    private bool changeAttack, changeMove, attackOn;
    public bool start;
    private BossTeleport bossT;
    private TeleportControl tpControl;
    public int state = 0;
    public int hit = 0;
    private Coroutine tpWait;
    private ColorStats weapon;
    private int attackPts, movePts;
    private Transform player;
    private Vector3 distance;
    private float count = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        attackOn = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackPts = movePts = 0;
        
        weapon = GameObject.Find("Canvas").GetComponent<ColorStats>();
        changeMove = changeAttack = false;
        attack1 = transform.GetChild(0).GetChild(0).GetComponent<LaserAttack>();
        attack2 = transform.GetChild(0).GetChild(0).GetComponent<LightningAttack>();
        move1 =  transform.GetComponent<MovementCorrector>();
        moveIP = transform.GetComponent<MovementInfinityPath>();
        tpControl = transform.GetComponent<TeleportControl>();
        bossT = transform.GetComponent<BossTeleport>();
        attack2.enabled = true;
    }
    //DATA HAS: Distance, Health Left, Weapon, 
    //attack 1: Distance Close, Health unimportant, weapon bow or shield;
    //attack 2: Distance Far, Health unimportant, weapon sword or shield;
    //move 1: Distance Far, Health unimportant, bow or shield;
    //move 2: Distance Close, Health important, sword or shield;

    private void calculatePts(){
        attackPts = movePts = 0;
        if(weapon.changeState == 0) movePts += 1; //  * GenerateRandomInt(1,3); 
        else if(weapon.changeState == 2) movePts -= 1; // * GenerateRandomInt(1,3);

        distance = transform.position - player.position;
        if((distance.x>5 || distance.x<-5) && (distance.y>5 || distance.y <-5)) movePts += 1; // * GenerateRandomInt(1,3);
        else movePts -= 1; // * GenerateRandomInt(1,10);

        if(hit > 5) movePts += 1; // * GenerateRandomInt(1,3);
        else if(hit < 3) movePts -= 1; //* GenerateRandomInt(1,2);

        if(movePts< 0 && moveState != 2) changeMove = true;
        else if(movePts > 0 && moveState != 1) changeMove = true;

        if(!changeMove && GenerateRandomInt(0,10) <= 3) changeMove = true;
    }

    public int GenerateRandomInt(int min, int max)
{
    using (var rng = new RNGCryptoServiceProvider())
    {
        var buffer = new byte[4];
        rng.GetBytes(buffer);
        var result = BitConverter.ToInt32(buffer, 0);

        return Mathf.RoundToInt(Mathf.Lerp(min, max, (result / (float)int.MaxValue)));
    }
}
    // Update is called once per frame
    void Update()
    {
        if(attackOn){
            switch(attack) 
            {
            case 1:
                if(attack1.stop){
                    attackOn = false;
                    calculatePts();
                    attack1.enabled = false;
                    if(changeMove){
                        attack++;
                        changeMove= false;
                    }
                    StartMovement();
                }
                break;
            case 2:
                if(attack2.stop){
                    attackOn = false;
                    calculatePts();
                    attack2.enabled = false;
                    if(changeMove){
                        attack--;
                        changeMove = false;
                    }
                    StartMovement();
                }
                break;
            }
        }
        else{
            switch(moveState) 
            {
            case 1:
                if(moveIP.stop){
                    attackOn = true;
                    calculatePts();
                    moveIP.enabled = false;
                    if(changeMove){
                        moveState++;
                        changeMove = false;
                    }
                    StartAttacks();
                }
                break;
            case 2:
                if(bossT.stop){
                    attackOn = true;
                    calculatePts();
                    bossT.enabled = false;
                    if(changeMove){
                        moveState--;
                        changeMove = false;
                    }
                    StartAttacks();
                }
                break;
            }
        }
        
    }

    public void StartMovement(){
        switch(moveState){
            case 1:
                moveIP.stop = false;
                move1.enabled = true;
                break;
            case 2:
                bossT.stop = false;
                tpControl.enabled = true;
                //start = true;
                break;
        }
    }
    public void StartAttacks(){
        switch(attack) 
        {
        case 1:
            attack1.Restart();
            attack1.enabled = true;
            break;
        case 2:
            attack2.Restart();
            attack2.enabled = true;
            break;
        case 3:
            break;
        }
    }

    IEnumerator WaitTeleport()
    {
        start = false;
        /*
        start = false;

        switch(state){
            case 0:
                yield return new WaitForSeconds(UnityEngine.Random.Range(5, 8));
                break;
            case 1:
                yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
                break;
            case 2:
                yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
                break;
        }
        StopAttacks();
        bossT.enabled = true;
        */
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));
        //bossT.enabled = true;
        
    }

    public void HitPoint(){
        hit++;
    }
}
