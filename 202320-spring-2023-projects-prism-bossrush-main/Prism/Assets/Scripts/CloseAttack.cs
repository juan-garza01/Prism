using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CloseAttack : MonoBehaviour
{
    private bool attack = false;
    private GameObject enemy, player;
    private EnemyStats stats;
    private Vector3 backLocation, dir, path;
    private AIPath aiPath;
    private float count, cooldown = 1f, moveSpeed = 6f, speed;
    // Start is called before the first frame update
    void Start()
    {
        stats = transform.GetChild(0).GetComponent<EnemyStats>();
        player = GameObject.Find("Player");
        enemy = transform.parent.gameObject;
        aiPath = enemy.GetComponent<AIPath>();
        count = cooldown;
        backLocation = enemy.transform.position;
        speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(attack){
            count -= Time.deltaTime;
            if(count <= -1.0){
                stats.hitbox.enabled = true;
                count = cooldown;
            }
            else if(count <= -.66){
                stats.idle = true;
                aiPath.canMove = false;
                path = backLocation - transform.position;
                dir = path.normalized;
                enemy.transform.position += (dir * moveSpeed * Time.deltaTime);
            }
            else if(count <= -.34){
                stats.hitbox.enabled = true;
            }
            else if (count <= -.33){
                stats.hitbox.enabled = false;
                //aiPath.canMove = true;
                path =  player.transform.position - transform.position;
                dir = path.normalized;
                enemy.transform.position += (dir * moveSpeed * Time.deltaTime);
                stats.idle = false;
                
            }
            else if (count <= 0){
                //
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            stats.dontmove = true;
            backLocation = enemy.transform.position;
            attack = true;
            aiPath.maxSpeed *= 2;
            aiPath.canMove = false;
            stats.onsight = true;
            stats.idle = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            stats.dontmove = false;
            attack = false;
            aiPath.canMove = true;
            aiPath.maxSpeed /= 2;
            count = 0;
            stats.onsight = false;
            stats.idle = false;
            stats.hitbox.enabled = true;
        }
    }
}