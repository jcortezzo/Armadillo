using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float JUMP_FORCE;
    [SerializeField] private float SPEED;
    [SerializeField] private float JUMP_THRESHHOLD;
    [SerializeField] private float BOUNCE_TIMER;

    private float canBounce;
    
    private Rigidbody2D rb;
    private Animator anim;
    private ParticleSystem jumpParticles;
    private ParticleSystem killParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpParticles = transform.GetChild(0).GetComponent<ParticleSystem>();  // uhh
        killParticles = transform.GetChild(1).GetComponent<ParticleSystem>();  // change this
        canBounce = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalManager.instance.IsGameStarted())
        {
            PollStart();
            return;
        }

        if (canBounce > 0)
        {
            canBounce -= Time.deltaTime;
        }

        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontal, 0);
        movement.Normalize();
        movement = new Vector2(movement.x * SPEED * Time.deltaTime, movement.y);

        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * JUMP_FORCE / rb.mass, ForceMode2D.Impulse);
        //jumpParticles.Play();
    }

    private void PollStart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalManager.instance.StartGame();
            Jump();
            anim.SetBool("Started", GlobalManager.instance.IsGameStarted());  // should be true
        }
    }

    private bool CanBounce()
    {
        return canBounce <= 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GlobalManager.instance.IsGameStarted() || !CanBounce() )
        {
            return;
        }

        foreach(ContactPoint2D point in collision.contacts)
        {
            if (point.normal.y >= JUMP_THRESHHOLD)
            {
                Jump();
                canBounce = BOUNCE_TIMER;

                Killable k = collision.gameObject.GetComponent<Killable>();
                if (k != null)
                {
                    killParticles.Play();
                    GlobalManager.instance.camController.Shake(0.1f, 0.25f, 1.0f);
                    Destroy(k.gameObject);
                }
                else
                {
                    jumpParticles.Play();
                }
                return;
            }
        }
    }
}
