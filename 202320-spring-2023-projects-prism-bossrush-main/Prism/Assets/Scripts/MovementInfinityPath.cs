using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInfinityPath : MonoBehaviour
{
    private float speed, radius, move, x, y;
    public float count, duration;
    private bool once;
    private Vector3 startPositon, target, origin;
    private bool backwards, canChange, right, rayRight, rayLeft, rayUp, rayDown, go;
    public LayerMask wallLayer;
    public float raycastDistance = 9f;
    public bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        Restart();
        //Reset();
    }

    

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if(duration > 0)
            MoveThisBoss();
        else{
            stop = true;
            this.enabled = false;
        }
    }

    public void Restart(){
        duration = Random.Range(5f, 10f); 
        once = true;
        right = true;
        canChange = false;
        backwards = true;
        speed = 1.5f;
        radius = 2.5f;
        count = -.11f;
        startPositon = transform.localPosition + new Vector3(radius,-radius,0);
        //origin = startPositon;
        Reset();
    }

    private void MoveThisBoss(){
        if(canChange){
            Rotation();
            count += Time.deltaTime;
            if(count >= 4.20){
                canChange = false;
                //goingUp = false;
                right = !right;
                Reset();
                if(right)
                    count = .0f;
                else
                    count = .60f;
                
            }
        }
        else{
            //count = -.11f;
            // Calculate the direction to the target position
            Vector3 direction = (target - transform.localPosition).normalized;

            // Move the enemy towards the target position at a normalized speed
            transform.localPosition += direction * speed* 2 * Time.deltaTime;

            direction = transform.localPosition - target;
            //count = -.11f;
            
            if(direction.x < 0.1 && direction.x > -0.1 && direction.y < 0.1 && direction.y > -0.1){
                canChange = true;
            }
        }
    }


    public void Reset(){
        radius *= -1;
        //startPositon = startPositon += new Vector3(2*radius,0,0);

        if(radius>0) backwards = true; 
        else backwards = false;
        move += Time.deltaTime * speed;
        move = radius * 3/4;

       moveLocation();
       
       target = startPositon + new Vector3(x,y,0);
    }

    private void Rotation(){
        
        move += Time.deltaTime * speed;
        moveLocation();
        transform.localPosition = (startPositon + new Vector3(x, y, 0));
    }

    private void moveLocation(){
        x = Mathf.Sin(move) * radius;
        y = Mathf.Cos(move) * radius;
        if(backwards){
            x -= radius*2;
        }
        else{
            x = -x;
        }
    }
    
}
