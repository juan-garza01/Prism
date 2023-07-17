using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    private bool canAttack;
    public bool stop;
    private HitArea hAttack;
    public GameObject startVFX;
    private AttackController attackMove;
    private Coroutine attackCor;
    private CrystalHealth crystalHP;
    // Start is called before the first frame update
    void Start()
    {
        Restart();
        //attackMove = transform.parent.parent.GetComponent<AttackController>();
        
    }

    public void Restart(){
        canAttack = true;
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack){
            //attackMove.StartMovement();
            canAttack = false;
            stop = false;
            
            attackCor = StartCoroutine((Attack()));
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        //attackMove.StopMovement();
        foreach (Transform child in transform) {
            if(child.gameObject.activeSelf) child.GetComponent<CrystalHealth>().enabled = false;
        }
        foreach (Transform child in transform)
        {
            if (!child){
                continue;
            }
            crystalHP = child.GetComponent<CrystalHealth>();
            if(crystalHP.health <= 0){
                continue;
            }
            startVFX = child.GetChild(1).GetChild(1).gameObject;
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            hAttack = child.GetChild(2).GetComponent<HitArea>();
            hAttack.Restart(startVFX.transform.GetChild(1).GetComponent<ParticleSystem>());
        }
        yield return new WaitForSeconds(5f);
        foreach (Transform child in transform)
                child.GetComponent<CrystalHealth>().enabled = true;

        stop = true;
        canAttack = true;
        this.enabled = false;
    }
}
