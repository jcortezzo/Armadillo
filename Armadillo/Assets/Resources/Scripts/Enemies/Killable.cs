using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    [SerializeField] private float score;
    [SerializeField] private float karma;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public float GetKarmaWorth()
    {
        return karma;
    }

    public float GetScoreWorth()
    {
        return score;
    }
}
