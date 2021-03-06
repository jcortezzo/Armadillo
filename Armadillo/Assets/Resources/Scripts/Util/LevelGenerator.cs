﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Room currentRoom;  // unserialize this when ready
    [SerializeField] private Room prevRoom;
    [SerializeField] private float GEN_THRESHHOLD;
    [SerializeField] private float DELETE_THRESHHOLD;

    [SerializeField] private GameObject[] rooms;

    [SerializeField] private GameObject[] defaultRooms;
    [SerializeField] private GameObject[] heavenRooms;
    [SerializeField] private GameObject[] hellRooms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DeletePrevRoom();
        GenerateNextRoom(); 
    }

    private GameObject[] ChooseRoomType()
    {
        Biomes b = GlobalManager.instance.GetBiome();
        if (b == Biomes.DEFAULT)
        {
            return defaultRooms;
        }
        else if (b == Biomes.HEAVEN)
        {
            return heavenRooms;
        }
        else  // all else hell (should be elif if compilers were smart)
        {
            return hellRooms;
        }
    }

    /// <summary>
    /// Generates a new room if the player is close enough to the next room.
    /// </summary>
    private void GenerateNextRoom()
    {
        if (!GlobalManager.instance.HasPlayer()) return;
        
        if (GlobalManager.instance.player.transform.position.x >=
            currentRoom.boundingBox.center.x - GEN_THRESHHOLD)
        {
            Room nextRoom = 
                    Instantiate(ChooseRoomType()[Random.Range(0, ChooseRoomType().Length)],   // was prev from rooms arr
                                new Vector2(currentRoom.boundingBox.max.x, currentRoom.transform.position.y), 
                                Quaternion.identity)
                                .GetComponent<Room>();
            prevRoom = currentRoom;
            currentRoom = nextRoom;
        }
    }

    /// <summary>
    /// Deletes a room if the player is far enough away from it.
    /// The player cannot move backwards so this is okay and efficient.
    /// </summary>
    private void DeletePrevRoom()
    {
        if (!GlobalManager.instance.HasPlayer()) return;
        if (prevRoom == null) return;

        if (GlobalManager.instance.player.transform.position.x >=
            currentRoom.boundingBox.center.x + DELETE_THRESHHOLD)
        {
            // detatch enemy children before destroying
            // so they can move between screens
            // NOTE: I wish I could do this in OnDestroy() in Room,
            //       but that causes undesirable behavior. It seems
            //       even after detaching in OnDestroy(), the Room
            //       still tries to delete all of its children at
            //       the time that Destroy() was called.
            foreach (Transform child in prevRoom.transform)
            {
                if (child.gameObject.CompareTag("Enemy") || 
                    child.gameObject.CompareTag("Holy"))
                {
                    child.SetParent(null);
                }
            }
            Destroy(prevRoom.gameObject);
        }
    }
}
