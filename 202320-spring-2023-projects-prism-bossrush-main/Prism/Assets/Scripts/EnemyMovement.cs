using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed = 3f;
    private Vector3 dir, path;
    [SerializeField] private AudioSource movingSound;
    public float movingCoolDown = 1f;
    public float moveTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        path = player.transform.position - transform.position;
        dir = path.normalized;
        if(Time.time > moveTime) {
            moveTime = Time.time + movingCoolDown;
            movingSound.Play();
        }
    }

    void FixedUpdate()
    {
        transform.position = transform.position + (dir * moveSpeed * Time.deltaTime);
    }
}
