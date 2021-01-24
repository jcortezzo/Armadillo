using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quantum : MonoBehaviour
{
    [SerializeField] protected bool isQuantum;
    [SerializeField] protected float probability = 0.5f;

    protected virtual void Awake()
    {
        if (isQuantum && Random.Range(0f, 1f) < probability)
        {
            Destroy(this.gameObject);
        }
    }
}
