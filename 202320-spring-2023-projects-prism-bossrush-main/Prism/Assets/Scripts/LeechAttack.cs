using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LeechAttack : MonoBehaviour
{
    public bool attack = false;
    private GameObject enemy, player;
    private EnemyStats stats;
    private Vector3 backLocation, dir, path;
    private AIPath aiPath;
    public float count = 0f; 
    private float cooldown = 0f, moveSpeed = 5f, speed;
    private HealthBar healthbar;
    private Shield shield;

    void Start()
    {
        stats = transform.GetChild(0).GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = transform.parent.gameObject;
        aiPath = enemy.GetComponent<AIPath>();
        speed = 5f;
        healthbar = player.GetComponent<HealthBar>();
        shield = player.transform.GetChild(2).GetComponent<Shield>();
    }

    void Update() {
        if(attack){
            count += Time.deltaTime;
            transform.GetChild(0).position = transform.position;

            if(count >= 10){
                stats.selfDestruct();
                this.enabled = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Weapon"){
            count = 0;
            attack = false;
            stats.animator.SetBool("Attached", false);
            aiPath.maxSpeed *= 0;
            aiPath.canMove = false;
        }
        if(other.tag == "Player"){
            stats.animator.SetBool("Attached", true);
            stats.dontmove = true;
            attack = true;
            aiPath.maxSpeed = 10;
            aiPath.canMove = false;
            stats.onsight = true;
            stats.idle = true;
        } 
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            count = 0;
            attack = false; 
            stats.animator.SetBool("Attached", false);
            stats.dontmove = false;
            aiPath.canMove = true;
            aiPath.maxSpeed = 2.5f;
            count = 0;
            stats.onsight = false;
            stats.idle = false;
            stats.hitbox.enabled = true;
        }
    }
}
