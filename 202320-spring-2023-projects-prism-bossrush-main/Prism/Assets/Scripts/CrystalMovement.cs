using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMovement : MonoBehaviour
{
    private float speedRotate = 110;
    private float orgSlow = 25;
    private float orgFast = 110;
    
    
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    public void speedSub(){
        speedRotate = orgSlow;
    }
    public void speedAdd(){
        speedRotate = orgFast;
    }

    public void increaseSpeed(){
        orgSlow *= 1.5f;
        orgFast *= 1.5f;
        speedRotate *= 1.5f;
    }
}
