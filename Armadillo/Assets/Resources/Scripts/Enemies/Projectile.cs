using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ParticleSystem deathParticles;
    private SpriteRenderer sr;
    private CircleCollider2D cc;

    // Start is called before the first frame update
    void Start()
    {
        deathParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        deathParticles.Play();
        float duration =
                deathParticles.duration + deathParticles.startLifetime;  // ???
        sr.enabled = false;
        cc.enabled = false;
        this.enabled = false;  // maybe sus, trying to get rid of bug where
                               // you can still land on the ground after dying 
        Destroy(this.gameObject, duration);
    }
}
