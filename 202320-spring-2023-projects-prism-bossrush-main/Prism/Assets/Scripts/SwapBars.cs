using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapBars : MonoBehaviour
{
    private ColorStats stats;
    private GameObject range;
    private GameObject melee;
    private GameObject player;
    private GameObject shield;
    private Shield block;
    private bool shieldWorks;
    // Start is called before the first frame update
    void Awake()
    {
        //make sure the gameobjects are ticked ON!
        stats = GameObject.Find("Canvas").GetComponent<ColorStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        range  = GameObject.Find("rotatePoint");
        melee = GameObject.Find("AttackArea");
        shield = GameObject.Find("block");
        block = shield.GetComponent<Shield>();
        melee.SetActive(true);
        shield.SetActive(false);
        range.SetActive(false);
        shieldWorks = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeWeapon(2,1); 
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeWeapon(0,2); 
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeWeapon(1,0);  
        }
    }

    private void changeWeapon(int num1, int num2){
        if(stats.changeState == num1){
                stats.colorSwapFoward(true);
            }
            else if(stats.changeState == num2){
                stats.colorSwapFoward(false);
            }
            //Debug.Log("space key was pressed");
            selected();
    }

    public void selected()
    {
        if (stats.changeState == 0)
            {
                if(shieldWorks)
                    block.disableShield();
                melee.SetActive(true);
                shield.SetActive(false);
                range.SetActive(false);
            }
        else if (stats.changeState == 1)
            {
                melee.SetActive(false);
                shield.SetActive(true);
                range.SetActive(false);
            }
        else if (stats.changeState == 2)
            {
                if(shieldWorks)
                    block.disableShield();
                melee.SetActive(false);
                shield.SetActive(false);
                range.SetActive(true);
            }
    }

}
