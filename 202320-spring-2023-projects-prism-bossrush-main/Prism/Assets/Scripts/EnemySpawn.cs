using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySpawn : MonoBehaviour
{
    public Transform enemy;
    private int enemyNum;
    public float minX = -8f, maxX = 8f, minY = -3f, maxY = 3f;
    private bool spawn;
    private EnemyStats es;

    GameObject enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemyNum = Random.Range(5,8);
        spawn = true;
        enemies = GameObject.Find("EnemyPool").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn){
            spawnEnemies();
            spawn = false;
        }
    }

    void spawnEnemies(){
        for (int i = 0; i < enemyNum; i++){
            Vector3 v = new Vector3(Random.Range(minX,maxX), Random.Range(minY, maxY), 0f);
            Transform enemy = enemies.transform.GetChild(i);
            es = enemy.GetComponent<EnemyStats>();
            //if(es.enemyID == 0){
                enemy.transform.gameObject.SetActive(true);
                enemy.position = v;
                enemy.GetComponent<AIPath>().canMove = true;
                enemy.GetComponent<AIPath>().maxSpeed = 3f;
            //}
            
            enemy.GetChild(0).GetComponent<EnemyStats>().enabled = true;
        }
        this.enabled = false;
    }
}
