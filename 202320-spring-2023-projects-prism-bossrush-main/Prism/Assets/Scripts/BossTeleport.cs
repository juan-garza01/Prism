using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    private Vector3 middle = new Vector3(0,1,0);
    public LayerMask wallLayer;
    private float raycastDistance = 80f;
    private float minLeft, minRight, minUp, minDown, x, y;
    private bool l, r, u, d;
    const string teleport = "Teleport";
    private Animator animator;
    private bool move = true;
    AnimatorStateInfo state;
    private GameObject cat;
    private Vector3 dir, dir2, previous, newLocation;

    private ParticleSystem ps;
    private Transform rotateC;
    private GameObject player;
    private AttackController attackMove;
    private CrystalHealth crystalHP;
    public bool stop;
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        animator = transform.GetChild(1).GetComponent<Animator>();
        cat = transform.GetChild(0).gameObject;
        rotateC = cat.transform.GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player");
        //attackMove = transform.GetComponent<AttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move) possibleMovement();
    }

    public void possibleMovement(){
        stop = false;
        for(int i = 0; i < 20; i++){
            r = moveCheck(Vector2.right, raycastDistance, out minRight, true);
            l = moveCheck(Vector2.left, raycastDistance, out minLeft, true);
            u = moveCheck(Vector2.up, raycastDistance, out minUp, false);
            d = moveCheck(Vector2.down, raycastDistance, out minDown, false);

            if(r && l && u && d){
                previous = transform.position;
                x = Random.Range(minLeft+5, minRight-5);
                y = Random.Range(minDown+5, minUp-5);
                newLocation = transform.position + new Vector3(x, y, 0);
                dir = newLocation - player.transform.position;
                dir2 = newLocation - previous;
                Debug.Log("Work");
                if(((dir.x > 3 || dir.x < -3) && (dir.y > 3 || dir.y < -3)) && ((dir2.x > 3 || dir2.x < -3) && (dir2.y > 3 || dir2.y < -3))){
                    StartCoroutine(TeleportBoth());
                    move = false; 
                    break;
                }
            }
        }
    }

    private bool moveCheck(Vector2 v, float dist, out float min, bool horizontal){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, v, dist, wallLayer);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, v * hit.distance, Color.red);
            if (horizontal) {
                min = hit.point.x - transform.position.x;
            } else {
                min = hit.point.y - transform.position.y;
            }
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, v * dist, Color.green);
            min = horizontal ? float.NegativeInfinity : float.PositiveInfinity;
            return false;
        }
    }

    IEnumerator TeleportBoth()
    {
        animator.Play(teleport);
        yield return new WaitForSeconds(.5f);
        cat.SetActive(false);
        yield return new WaitForSeconds(.5f);
        transform.localPosition += new Vector3(x, y, 0);
        yield return new WaitForSeconds(.75f);
        animator.Play(teleport);
        yield return new WaitForSeconds(.5f);
        cat.SetActive(true);
        foreach (Transform child in rotateC)
        {
            if(child.gameObject.activeSelf == false){
                //child.gameObject.SetActive(true);
                
            }
            child.GetChild(0).GetComponent<LaserBeam>().DisableLaser();
            crystalHP = child.GetComponent<CrystalHealth>();
            if(crystalHP.health <= 0) continue;
            crystalHP.AnimationState();
            
            crystalHP.enabled = true;
        }
        yield return new WaitForSeconds(.5f);
        //attackMove.StartAttacks();
        //attackMove.BackToMovement();
        //attackMove.start = true;
        stop = true;
        move = true;
        this.enabled = false;
    }
}
