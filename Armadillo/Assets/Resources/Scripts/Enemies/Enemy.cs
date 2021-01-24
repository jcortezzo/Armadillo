using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Quantum
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Orients self left or right depending on the input vector.
    /// </summary>
    /// <param name="direction">Direction used to decide orientation (left or right facing).</param>
    protected void OrientCorrectly(Vector2 direction)
    {
        // WARNING: Make SURE everything has a scale of 1 first!!
        if (direction.x < 0 && !IsFacingLeft())
        {
            this.transform.localScale += new Vector3(2, 0, 0);  // janky as hell lol
        }
        else if (direction.x > 0 && IsFacingLeft())
        {
            this.transform.localScale += new Vector3(-2, 0, 0);
        }
    }

    private bool IsFacingLeft()
    {
        return this.transform.localScale.x > 0;
    }

    private void OnBecameInvisible()
    {
        // don't want to destroy enemies loaded in next room
        if (transform.parent == null) Destroy(this.gameObject);
    }
}
