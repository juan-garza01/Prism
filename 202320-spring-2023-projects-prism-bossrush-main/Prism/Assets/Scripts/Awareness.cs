using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Awareness : MonoBehaviour
{
    private AIPath aiPath;
    private bool mightMove = false;
    private Vector3 rotation;
    public LayerMask hitAreas;
    private GameObject player;
    void Start(){
        aiPath = this.GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        if(mightMove == true){
            rotation = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(rotation.x, rotation.y),10f, hitAreas);
            Debug.DrawRay(transform.position, new Vector2(rotation.x, rotation.y), Color.red,0, true);

            if(hit && hit.transform.gameObject == player){
                aiPath.canMove = true;
            }
            else{
                aiPath.canMove = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            mightMove = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player"){
            mightMove = false;
            aiPath.canMove = false;
        }
    }
}
