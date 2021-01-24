using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    [SerializeField] private float SPEED;
    private Rigidbody2D rb;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        OrientCorrectly(direction);

        // snakes aren't affected by gravity... kind of sus
        rb.velocity = new Vector2(direction.x * SPEED, rb.velocity.y);                                                                       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flip"))
        {
            direction *= -1;  // flip direction
        }
    }
}
