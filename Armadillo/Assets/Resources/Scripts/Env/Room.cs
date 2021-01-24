using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Bounds boundingBox;

    // Start is called before the first frame update
    void Start()
    {
        boundingBox = GetBoundingBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Bounds GetBoundingBox()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(transform.position, Vector3.zero);

        Bounds b = renderers[0].bounds;
        foreach(Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }

    //private void OnBecameInvisible()
    //{
    //    Destroy(this.gameObject);
    //}
}
