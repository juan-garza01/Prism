using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCorrector : MonoBehaviour
{
    private bool rayRight, rayLeft, rayUp, rayDown, go;
    public float raycastDistance = 9f;
    public LayerMask wallLayer;
    public float speed = 1.5f;
    private Vector3 middle = new Vector3(0,1,0);
    private MovementInfinityPath movePath;
    public AttackController attackMove;
    // Start is called before the first frame update
    void Start()
    {
        movePath = transform.GetComponent<MovementInfinityPath>();
        attackMove = transform.GetComponent<AttackController>();
        //FixThisBoss();
    }

    // Update is called once per frame
    void Update()
    {
        
        FixThisBoss();
        if(go){
            movePath.Restart();
            movePath.enabled = true;
            attackMove.StartAttacks();
            this.enabled = false;
        }
        

    }

    public bool possibleMovement(){
        rayRight = moveCheck(Vector2.right, raycastDistance);
        rayLeft =moveCheck(Vector2.left, raycastDistance);
        rayUp = moveCheck(Vector2.up, raycastDistance);
        rayDown = moveCheck(Vector2.down, raycastDistance);
        
        if(rayRight && rayLeft && rayUp && rayDown)
            return true;
        else
            return false;
    }

    private void FixThisBoss(){
        Vector3 direction = Vector3.zero;
        if(rayRight) direction += Vector3.right;
        if(rayLeft) direction += Vector3.left;
        if(rayUp) direction += Vector3.up;
        if(rayDown) direction += Vector3.down;
        direction.Normalize();
        transform.localPosition += direction * speed * Time.deltaTime;
        
        go = possibleMovement();
    }

    private bool moveCheck(Vector2 v, float dist){
        RaycastHit2D hit = Physics2D.Raycast(transform.localPosition+middle, v, dist, wallLayer);

        // Check if the ray hit a wall
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.localPosition+middle, v* hit.distance, Color.red);
            //Debug.Log("Wall hit!");
            return false;
        }
        else
        {
            Debug.DrawRay(transform.localPosition+middle, v * dist, Color.green);
            return true;
        }
    }
}
