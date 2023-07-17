using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private bool shooting;
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public LayerMask hitList;
    public GameObject startVFX;
    public GameObject endVFX;
    public LaserVisuals laserV;
    public GameObject player;
    public HealthBar hp;
    private bool canHit;
    public float hitCooldown = 0f;
    private CrystalMovement cMove;
    public bool end;
    private RaycastHit2D hit;
    private GameObject yPool, healthPool;

    private int rIntNum, choice;
    private Transform ammo, heart;
    private float randomNum;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    // Start is called before the first frame update
    void Start()
    {
        yPool = GameObject.Find("YellowEnergyPool");
        healthPool = GameObject.Find("HealthPool");
        player = GameObject.FindGameObjectWithTag("Player");
        hp = player.GetComponent<HealthBar>();
        cMove = transform.parent.parent.GetComponent<CrystalMovement>();
        Restart();
    }

    public void Restart(){
        end = false;
        FillLists();
        shooting = false;
        DisableLaser();
        canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(shooting){
            UpdateLaser();
        }
    }

    private void EnableLaser(){
        if(end) return;
        laserV.rotateFix();
        for(int i = 0; i <particles.Count; i++){
            particles[i].Play();
        }
        laserV.enabled = true;
        lineRenderer.enabled = true;
    }
    private void UpdateLaser(){
        var distance = transform.position - transform.parent.parent.position;
        lineRenderer.SetPosition(0, (Vector2)firePoint.position);
        startVFX.transform.position = (Vector2)firePoint.position;
        distance *= 10;
        distance.z = 0;
        lineRenderer.SetPosition(1, (Vector2)firePoint.position + new Vector2(distance.x,distance.y));
        
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, distance.normalized, 10);
        hit = Physics2D.Raycast(transform.position, new Vector2(distance.x, distance.y).normalized, 30f, hitList);
        if(hit){
            lineRenderer.SetPosition(1, hit.point);
        }
        endVFX.transform.position = lineRenderer.GetPosition(1);

        if(canHit && hit && hit.transform.tag == "Player"){
            canHit = false;
            hitCooldown = 0f;
            hp.hit(10);
        }
        else if(!canHit){
            hitCooldown += Time.deltaTime;
            if(hitCooldown >= 1){
                canHit = true;
            }
        }
    }
    public void DisableLaser(){
        lineRenderer.enabled = false;
        for(int i = 0; i <particles.Count; i++){
            particles[i].Stop();
        }
        laserV.rotateOff();
        laserV.enabled = false;
    }

    private void FillLists(){
        for(int i = 0; i < startVFX.transform.childCount; i++){
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null){
                particles.Add(ps);
            }
        }

        for(int i = 0; i < endVFX.transform.childCount; i++){
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null){
                particles.Add(ps);
            }
        }
    }

    public void Shoot(){
        StartCoroutine(ShootingWait());
    }
    public void StopShooting(){
        if(hit != false){
            randomNum = Random.Range(0, 100) * Time.deltaTime * 100;
            rIntNum = (int)randomNum%10;
            
            if(rIntNum == 1 || rIntNum == 4){
                ammo = yPool.transform.GetChild(0);
                choice = 0;
            }
            else if( rIntNum == 7){
                heart = healthPool.transform.GetChild(0);
                choice = 1;   
            }
            else{
                choice = 2;
            }

            if(choice == 0){
                ammo.parent = null;
                ammo.position = this.transform.position;
            }
            else if(choice == 1)
            {
                heart.parent = null;
                heart.position = this.transform.position;
            }
            else if (choice == 2){
                Debug.Log("Nothing");
            }
        }
        cMove.speedAdd();
        shooting = false;
        DisableLaser();
       
    }
    IEnumerator ShootingWait()
    {
        cMove.speedSub();
        var ps = startVFX.transform.GetChild(1).GetComponent<ParticleSystem>();
        ps.Play();
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);
        FillLists();
        shooting = true;
        EnableLaser();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}


