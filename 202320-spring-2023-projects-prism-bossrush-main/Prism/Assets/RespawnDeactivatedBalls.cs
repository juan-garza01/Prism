using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnDeactivatedBalls : MonoBehaviour
{
    private float count = 0;
    private float cooldown = 1;
    private YellowBossHealth bossHP;
    private LaserBeam lb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bossHP = gameObject.transform.parent.GetComponent<YellowBossHealth>();
        lb = transform.GetChild(0).GetComponent<LaserBeam>();
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count > cooldown){
            count = 0;
            foreach (Transform child in transform) {
                if(child.gameObject.activeSelf) continue;
                bossHP.Respawn(child.GetComponent<CrystalHealth>(), child.transform.GetChild(0).GetComponent<LaserBeam>());
                child.gameObject.SetActive(true); 
                    
            }
        }
    }
}
