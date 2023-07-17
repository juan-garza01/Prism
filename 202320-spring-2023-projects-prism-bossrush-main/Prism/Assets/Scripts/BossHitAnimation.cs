using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitAnimation : MonoBehaviour
{
    private Animator animator;
    const string playerDeath = "Death_Explosion";
    const string ballHit = "Hit";
    private Transform crystalRotate;
    private LaserAttack attack1;
    private CrystalMovement cMove;
    private AttackController attackC;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;
    public GameManagerScript gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").transform.GetComponent<GameManagerScript>();
        animator = GetComponent<Animator>();
        crystalRotate = transform.GetChild(0).GetChild(0);
        attack1 = crystalRotate.GetComponent<LaserAttack>();
        cMove = crystalRotate.GetComponent<CrystalMovement>();
        attackC = transform.GetComponent<AttackController>();

    }
    public void PlayHit(){
        attackC.HitPoint();
        animator.Play(ballHit);
        hitSound.Play();
    }

    public void PlayDeath(){
        deathSound.Play();
        cMove.enabled = false;
        attack1.enabled = false;
        attack1.LaserShootStop();
        animator.Play(playerDeath);
        foreach (Transform child in crystalRotate)
        {
            child.GetComponent<Animator>().Play("BrokenCrystal");
        }
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(.66f);
        gm.Win();
        Destroy(gameObject);
    }
}
