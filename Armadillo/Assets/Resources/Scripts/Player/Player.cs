using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float JUMP_FORCE;
    [SerializeField] private float SPEED;
    [SerializeField] private float JUMP_THRESHHOLD;
    [SerializeField] private float BOUNCE_TIMER;
    [SerializeField] private float HEAVEN_KARMA;
    [SerializeField] private float HELL_KARMA;

    private float canBounce;
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private ParticleSystem jumpParticles;
    private ParticleSystem killParticles;
    private ParticleSystem deathParticles;

    [Header("Iframes")]
    [SerializeField] private int NUM_FLASHES;
    [SerializeField] private float FLASH_DURATION;
    private bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        jumpParticles = transform.GetChild(0).GetComponent<ParticleSystem>();  // uhh
        killParticles = transform.GetChild(1).GetComponent<ParticleSystem>();  // changed
        deathParticles = transform.GetChild(1).GetComponent<ParticleSystem>();  // change this
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

    /// <summary>
    /// Moves the player horizontally.
    /// </summary>
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontal, 0);
        movement.Normalize();
        movement = new Vector2(movement.x * SPEED, movement.y);

        if (GlobalManager.instance.cam.WorldToScreenPoint(transform.position).x <= 0)
        {
            movement = new Vector2(Mathf.Max(movement.x, 0), movement.y);
        }

        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * JUMP_FORCE / rb.mass, ForceMode2D.Impulse);
        //jumpParticles.Play();
    }

    /// <summary>
    /// Initial check to see if the game should start.
    /// </summary>
    private void PollStart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalManager.instance.StartGame();
            Jump();
            anim.SetBool("Started", GlobalManager.instance.IsGameStarted());  // should be true
        }
    }

    /// <summary>
    /// Returns whether the player can bounce. Mostly
    /// serves as a bug fix so a player can't bounce
    /// twice if hitting two objects from the top at once.
    /// </summary>
    /// <returns>Whether the player can bounce or not.</returns>
    private bool CanBounce()
    {
        return canBounce <= 0;
    }

    /// <summary>
    /// Player death.
    /// </summary>
    private void Die()
    {
        invincible = true;
        //StartCoroutine(Death());
        deathParticles.Play();
        
        GlobalManager.instance.camController.Shake(0.1f, 0.25f, 1.0f);
        //GlobalManager.instance.palette.SetColors(GlobalManager.HELL_PALETTE);
        GlobalManager.instance.SetBiome(ChooseBiome());
        //
        Jukebox.Instance.PlaySFX("die", 0.3f, 1f);
        GlobalManager.instance.AddLives(-1);

        // actually dead
        if (GlobalManager.instance.GetLives() < 0)
        {
            float duration =
                deathParticles.duration + deathParticles.startLifetime;  // ???
            sr.enabled = false;
            this.enabled = false;  // maybe sus, trying to get rid of bug where
                                   // you can still land on the ground after dying 
            Destroy(this.gameObject, duration);

            GlobalManager.instance.EndCurrentGame();
        } else
        {
            Jump();
            StartCoroutine(TakeDamage());
        }
    }

    /// <summary>
    /// Returns the next biome based on your karma. If you're on Earth
    /// you will either go to heaven or hell. If you are in heaven or hell
    /// you will go back to Earth unless you have a hell-worthy karma in heaven.
    /// </summary>
    /// <returns>The correct biome to travel to after death.</returns>
    private Biomes ChooseBiome()
    {
        Biomes currentBiome = GlobalManager.instance.GetBiome();
        int karma = GlobalManager.instance.GetKarma();

        if (currentBiome == Biomes.DEFAULT)
        {
            return karma >= HEAVEN_KARMA ? Biomes.HEAVEN : Biomes.HELL;
        }
        else if (currentBiome == Biomes.HEAVEN)
        {
            return karma < HELL_KARMA ? Biomes.HELL : Biomes.DEFAULT;  // maybe LEQ instead of strictly LE?
        }
        else
        {
            return Biomes.DEFAULT;
        }
    }

    /// <summary>
    /// Don't manage collision if the game hasn't started.
    /// Kills player if player runs into dangerous things.
    /// Player will bounce off of objects that it can bounce off of.
    /// </summary>
    /// <param name="collision">Object the player is colliding with.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GlobalManager.instance.IsGameStarted())
        {
            return;
        }

        /*
         * TODO: if touching lava or spikes player should always die
         */
        if (collision.gameObject.CompareTag("OHKO") && !invincible)
        {
            Die();
            return;
        }

        // if not touching lava nor spikes, test if we can
        // jump off of the object we're touching
        if (CanBounce())
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y >= JUMP_THRESHHOLD)
                {
                    Jump();
                    canBounce = BOUNCE_TIMER;
                    Jukebox.Instance.PlaySFX("Jump", 0.5f, 1f);

                    Killable k = collision.gameObject.GetComponent<Killable>();
                    if (k != null)
                    {
                        killParticles.Play();
                        GlobalManager.instance.camController.Shake(0.1f, 0.25f, 1.0f);
                        GlobalManager.instance.AddScore(k.GetScoreWorth());
                        GlobalManager.instance.AddKarma(k.GetKarmaWorth());

                        if (k.GetKarmaWorth() != 0)
                        {
                            UIManager.instance.PopUpKarma(k.gameObject, Mathf.FloorToInt(k.GetKarmaWorth()),
                                                          k.GetKarmaWorth() > 0 ? Color.white : Color.red);
                        }
                        
                        Jukebox.Instance.PlaySFX("Dmg2", 0.5f, 1f);
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

        // if not on top of other object
        // see if it will kill you
        if (collision.gameObject.CompareTag("Enemy") && !invincible)
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Holy"))
        {
            collision.gameObject.GetComponent<Angel>().Bless();  // LMF
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OHKO") && !invincible)
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Holy"))
        {
            collision.gameObject.GetComponent<Apple>().Bless();  //AO
            collision.gameObject.GetComponent<Apple>().Use();
        }
    }

    private IEnumerator TakeDamage()
    {
        int n = NUM_FLASHES;
        invincible = true;
        while (n --> 0)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(FLASH_DURATION);
            sr.enabled = true;
            yield return new WaitForSeconds(FLASH_DURATION);
        }
        invincible = false;
    }
}
