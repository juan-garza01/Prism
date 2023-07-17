using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBossHealth : MonoBehaviour
{
    public int bossHealth;
    private BossHitAnimation bossHit;
    
    public GameObject crystalPrefab, childBall;

    private float[] orgRotationArr = new float[3]; 
    public CrystalMovement cMove;
    private TeleportControl tpBoss;
    //public Animation explosionDeath;
    // Start is called before the first frame update
    void Start()
    {
        bossHealth = 9;
        bossHit = transform.parent.GetComponent<BossHitAnimation>();
        cMove = transform.GetChild(0).GetComponent<CrystalMovement>();
        tpBoss = transform.parent.GetComponent<TeleportControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lessHealth(){
        bossHealth--;
        if(bossHealth == 0)
            bossHit.PlayDeath();
        else
            bossHit.PlayHit();
        if(bossHealth == 6 || bossHealth == 3){
            cMove.increaseSpeed();
            tpBoss.state++;
        }
    }

    public void Respawn(CrystalHealth cHP, LaserBeam lb){
        if(bossHealth >= 6) StartCoroutine(CreateBall(cHP, lb));
        else Destroy(cHP.gameObject);
    }

    IEnumerator CreateBall(CrystalHealth cHP, LaserBeam lb)
    {
        yield return new WaitForSeconds(1f);
        cHP.RepoolArrows();
        cHP.Restart();
        lb.Restart();
        
    }


    
}
