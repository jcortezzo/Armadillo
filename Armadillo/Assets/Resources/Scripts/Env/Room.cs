using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Bounds boundingBox;

    // Start is called before the first frame update
    void Start()
    {
        // should only need to calculate once
        // if you keep all env inside of the box
        // (I think I will do that 😊)
        boundingBox = GetBoundingBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a bounding box over all objects in the room.
    /// </summary>
    /// <returns>Bounds covering all objects in the room.</returns>
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

    // NOTE: See LevelGenerator for explanation
    //private void OnDestroy()
    //{
    //    // detatch enemy children before destroying
    //    // so they can move between screens
    //    foreach (Transform child in transform)
    //    {
    //        if (child.gameObject.CompareTag("Enemy"))
    //        {
    //            child.SetParent(null);
    //        }
    //    }
    //}
}
