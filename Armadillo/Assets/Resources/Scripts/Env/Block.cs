using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Quantum
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Type", Random.Range(0, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
