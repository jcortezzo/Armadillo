using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float SPEED;
    private bool seen;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seen = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalManager.instance.IsGameStarted()) return;

        Move();
    }

    private void Move()
    {
        if (!seen) return;
        if (!GlobalManager.instance.HasPlayer())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 movement = 
                GlobalManager.instance.player.transform.position - this.transform.position;
        movement.Normalize();

        rb.velocity = movement * SPEED * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (seen) return;

        Player p = other.gameObject.GetComponent<Player>();

        if (p != null)
        {
            seen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p != null)
        {
            //Debug.Log("player triggered");
            seen = true;
        }
    }
}
