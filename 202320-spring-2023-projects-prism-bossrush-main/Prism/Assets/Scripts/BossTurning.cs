using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurning : MonoBehaviour
{
    private GameObject player;
    private Vector3 distance;
    public Animator animate;
    // Start is called before the first frame update
    void Start()
    {
        distance = new Vector3(0,0,0);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = player.transform.position - transform.position;
        animate.SetFloat("X", distance.x);
        animate.SetFloat("Y", distance.y);
    }
}
