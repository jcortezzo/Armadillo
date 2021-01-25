using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    private Animator anim;
    [SerializeField] private GameObject projectileGO;
    private static readonly Vector2[] corners =
    {
        (Vector2.up + Vector2.right).normalized,  // NE
        (Vector2.up + Vector2.left).normalized,  // NW
        (Vector2.down + Vector2.right).normalized,  // SE
        (Vector2.down + Vector2.left).normalized,  // SW
    };

    [Header("Projectile Properties")]
    [SerializeField] private float PROJECTILE_SPEED;
    [SerializeField] private float FREQ;
    [SerializeField] private int NUM_CHARGES;
    [SerializeField] private float REFRACTORY;

    private float freqTimer;
    private int chargesLeft;
    private float refractoryTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootLogic();
    }

    private void ShootLogic()
    {
        if (!anim.GetBool("isOpened")) return;

        if (chargesLeft <= 0)
        {
            refractoryTimer -= Time.deltaTime;
            if (refractoryTimer <= 0)
            {
                chargesLeft = NUM_CHARGES;
            }
        }
        else if (freqTimer <= 0)
        {
            Shoot();
            if (chargesLeft <= 0)
            {
                refractoryTimer = REFRACTORY;
            }
        }
        else
        {
            freqTimer -= Time.deltaTime;
        }
    }

    private void Open()
    {
        anim.SetBool("isOpened", true);
    }

    private void Shoot()
    {
        foreach (Vector2 dir in corners)
        {
            GameObject projectile = Instantiate(projectileGO, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = dir * PROJECTILE_SPEED;
        }
        chargesLeft--;
        freqTimer = FREQ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p != null)
        {
            //Debug.Log("player triggered");
            anim.SetBool("isDetected", true);
        }
    }
}
