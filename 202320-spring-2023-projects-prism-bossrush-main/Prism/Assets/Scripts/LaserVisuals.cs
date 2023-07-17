using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class LaserVisuals : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public RotationConstraint rotateconst;
    private Quaternion rotation;
    public float orgRotate;
    // Start is called before the first frame update
    void Awake()
    {
        Restart();
        
    }

    private void Restart(){
        rotateconst = this.GetComponent<RotationConstraint>();
        orgRotate = start.transform.rotation.z;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rotation;
        start.transform.rotation = transform.parent.rotation;
        start.transform.rotation *= Quaternion.Euler(0,0,orgRotate);
        end.transform.rotation = transform.parent.rotation;
        //start.transform.rotation;
    }

    public void rotateFix(){
        rotateconst.enabled = false;
    }
    public void rotateOff(){
        rotateconst.enabled = true;
    }
}
