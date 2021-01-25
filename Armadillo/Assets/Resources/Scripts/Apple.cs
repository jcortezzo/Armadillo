using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Quantum
{
    [SerializeField] private float appleKarma;
    private SpriteRenderer sr;
    private bool used;

    // Start is called before the first frame update
    void Start()
    {
        used = false;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bless()
    {
        //deathParticles.Play();
        /*float duration =
                deathParticles.duration + deathParticles.startLifetime;  // ???*/
        sr.enabled = false;
        //cc.enabled = false;
        //this.enabled = false;  // maybe sus, trying to get rid of bug where
        // you can still land on the ground after dying 
        //GlobalManager.instance.AddKarma(appleKarma);
        if (used) return;

        GlobalManager.instance.AddLives(1);
        UIManager.instance.PopUpText(gameObject, "Life +1", Color.white);
        Destroy(this.gameObject, 0.25f);
    }

    public void Use()
    {
        used = true;
    }
}
