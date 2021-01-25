using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    private ParticleSystem deathParticles;
    private SpriteRenderer sr;
    private CircleCollider2D cc;

    [SerializeField] private float angelKarma;

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

    public void Bless()
    {
        deathParticles.Play();
        float duration =
                deathParticles.duration + deathParticles.startLifetime;  // ???
        sr.enabled = false;
        cc.enabled = false;
        //this.enabled = false;  // maybe sus, trying to get rid of bug where
        // you can still land on the ground after dying 
        GlobalManager.instance.AddKarma(angelKarma);
        UIManager.instance.PopUpKarma(gameObject, Mathf.FloorToInt(angelKarma), Color.white);
        Destroy(this.gameObject, duration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player p = collision.gameObject.GetComponent<Player>();

        //if (p != null)
        //{
        //    deathParticles.Play();
        //    float duration =
        //            deathParticles.duration + deathParticles.startLifetime;  // ???
        //    sr.enabled = false;
        //    cc.enabled = false;
        //    //this.enabled = false;  // maybe sus, trying to get rid of bug where
        //                           // you can still land on the ground after dying 
        //    GlobalManager.instance.AddKarma(angelKarma);
        //    UIManager.instance.PopUpKarma(gameObject, Mathf.FloorToInt(angelKarma), Color.white);
        //    Destroy(this.gameObject, duration);
        //}
    }
}
