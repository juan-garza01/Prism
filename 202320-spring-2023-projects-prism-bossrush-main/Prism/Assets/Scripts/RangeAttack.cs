using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    private Camera cam;
    private Vector3 mouse, rotation;
    private float rotateZ;
    public GameObject arrowPool;
    public Transform arrowTransform, arrow;
    public bool canShoot;
    private float timer;
    private float cooldown = .75f;
    private ColorStats cstat;
    private bool onceOn, onceOff, colorActive;
    private SpriteRenderer bowImage;
    public Sprite[] spriteArray;
    [SerializeField] private AudioSource bowshot;
    public PlayerMovement pm;
    private GameManagerScript gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").transform.GetComponent<GameManagerScript>();
        onceOn = onceOff = true;
        colorActive = false;
        cam = Camera.main;
        arrowPool = GameObject.Find("ArrowPool");
        bowImage = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        cstat = GameObject.Find("Canvas").GetComponent<ColorStats>();
        pm = transform.parent.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!gm.isPaused){
            if(cstat.yellow <= 0){
                onceOn = true;
                if(onceOff){
                    bowImage.sprite = spriteArray[0];
                    onceOff = false;
                }
            }
            else{
                onceOff = true;
                if(onceOn){
                    bowImage.sprite = spriteArray[1];
                    onceOn = true;
                }
            }
                    
            if (pm.facingRight == false)
            {

                mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                rotation = mouse - transform.position;
                rotateZ = Mathf.Atan2(-1 * rotation.y, -1 * rotation.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            }
            else if (pm.facingRight == true)
            {

                mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                rotation = mouse - transform.position;
                rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotateZ);
            }
            if(!canShoot)
            {
                timer += Time.deltaTime;
                if(timer > cooldown)
                {
                    canShoot = true;
                    timer = 0;
                }
            }

            if(Input.GetMouseButton(0) && canShoot)
            {
                canShoot = false;
                arrow = arrowPool.transform.GetChild(0);
                arrow.gameObject.SetActive(true);
                arrow.GetComponent<ArrowScript>().enabled = true;
                arrow.transform.position = transform.GetChild(0).position;
                
                bowshot.Play();
            }
        }

    }
}