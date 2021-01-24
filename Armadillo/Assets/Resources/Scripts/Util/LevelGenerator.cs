using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Room currentRoom;  // unserialize this when ready
    [SerializeField] private Room prevRoom;
    [SerializeField] private float GEN_THRESHHOLD;
    [SerializeField] private float DELETE_THRESHHOLD;

    [SerializeField] private GameObject[] rooms;


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

    private void GenerateNextRoom()
    {
        if (!GlobalManager.instance.HasPlayer()) return;
        
        if (GlobalManager.instance.player.transform.position.x >=
            currentRoom.boundingBox.center.x - GEN_THRESHHOLD)
        {
            Room nextRoom = 
                    Instantiate(rooms[Random.Range(0, rooms.Length)], 
                                new Vector2(currentRoom.boundingBox.max.x, currentRoom.transform.position.y), 
                                Quaternion.identity)
                                .GetComponent<Room>();
            prevRoom = currentRoom;
            currentRoom = nextRoom;
        }
    }

    private void DeletePrevRoom()
    {
        if (!GlobalManager.instance.HasPlayer()) return;
        if (prevRoom == null) return;

        if (GlobalManager.instance.player.transform.position.x >=
            currentRoom.boundingBox.center.x + DELETE_THRESHHOLD)
        {
            Destroy(prevRoom.gameObject);
        }
    }
}
