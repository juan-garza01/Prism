using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControl : MonoBehaviour
{
    //public AttackController attackMove;
    private bool start;
    private BossTeleport bossT;
    public int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        start = true;
        bossT = transform.GetComponent<BossTeleport>();
    }

    // Update is called once per frame
    void Update()
    {
        if(start) StartCoroutine(WaitTeleport());
    }

    IEnumerator WaitTeleport()
    {
        start = false;
        yield return new WaitForSeconds(WaitTime());
        bossT.enabled = true;
        start = true;
        this.enabled = false;
        
    }
    private float WaitTime(){
        switch(state){
            case 0:
                return Random.Range(5, 8);
            case 1:
                return Random.Range(3, 6);
            case 2:
                return Random.Range(2, 4);
        }
        return 0;
    }

    
}
