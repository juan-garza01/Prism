using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int force;
    //private Coroutine r = null;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Weapon")
        {
            StopCoroutine(reset());
            Vector2 dir = (transform.position - other.transform.position).normalized;
            rb.AddForce(dir * force, ForceMode2D.Impulse);            
            StartCoroutine(reset());
        }    
    }

    private IEnumerator reset() 
    {
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector3.zero;
        transform.parent.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}