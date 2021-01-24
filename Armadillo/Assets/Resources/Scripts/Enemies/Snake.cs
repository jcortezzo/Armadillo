using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
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
        if (direction.x < 0 && !IsFacingLeft())
        {
            this.transform.localScale += new Vector3(2, 0, 0);  // janky as hell lol
        }
        else if (direction.x > 0 && IsFacingLeft())
        {
            this.transform.localScale += new Vector3(-2, 0, 0);
        }
        rb.velocity = new Vector2(direction.x * SPEED * Time.deltaTime, 0);
    }

    private bool IsFacingLeft()
    {
        return this.transform.localScale.x > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flip"))
        {
            direction *= -1;  // flip direction
        }
    }
}
