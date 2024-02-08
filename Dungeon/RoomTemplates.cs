using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [Header("Dungeon Generation")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;    
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject onlyBottomRooms;
    public GameObject onlyTopRooms;
    public GameObject onlyLeftRooms;
    public GameObject onlyRightRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms;
    public GameObject end;

    public int roomsUpperLimit;
    public int roomsLowerLimit;
    [HideInInspector]
    public int maxRooms;
    public int currentRooms;

    [Header("Minimap")]
    public Transform miniMapCamera;
    public float camSizeMultiplier;

    [Header("Enemy Spawning")]
    public GameObject enemyRoom;
    public int enemySpawnChance; // all spawn chances must add to 100
    [Header("Chests")]
    public GameObject chest;
    public int chestSpawnChance;

    private void Start() {
        maxRooms = Random.Range(roomsLowerLimit, roomsUpperLimit);
        miniMapCamera = GameObject.FindGameObjectWithTag("MinimapCamera").transform;
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator WaitForSpawn(){
        yield return new WaitForSeconds(0.5f);
        Instantiate(end, rooms[rooms.Count-1].transform.position, Quaternion.identity);
        GenerationCheck();
        DetermineEnemyRooms();
        SpawnChests();
    }

    private void Update() {
        UpdateMinimapCameraPosition();
    }

    void GenerationCheck(){
        List<GameObject> roomsToAdd = new List<GameObject>();
        List<GameObject> roomsToRemove = new List<GameObject>();
        foreach (GameObject room in rooms){
            string roomName = room.name;
            foreach (Transform child in room.transform){
                if (child.gameObject.CompareTag("Collider")){
                    BoxCollider2D collider = child.gameObject.GetComponent<BoxCollider2D>();
                    if (collider.enabled){
                        GameObject chosenRoom = DetermineSide(collider, roomName);
                        Transform chosenRoomTransform = room.transform;
                        roomsToRemove.Add(room);
                        GameObject newRoom = Instantiate(chosenRoom, chosenRoomTransform.position, Quaternion.identity);
                        newRoom.transform.SetParent(GameObject.FindGameObjectWithTag("Grid").transform);
                        roomsToAdd.Add(newRoom);
                    }
                }
            }
        }
        rooms.AddRange(roomsToAdd);
        
        foreach (GameObject room in roomsToRemove){
            rooms.Remove(room);
            Destroy(room);
        }
    }

    GameObject DetermineSide(BoxCollider2D collider, string roomName){
        if (collider.offset.x != 0){
            if (collider.offset.x > 0){
                roomName = roomName.Replace("R", string.Empty);
            } else {
                roomName = roomName.Replace("L", string.Empty);;
            }
        }else{
            if (collider.offset.y > 0){
                roomName = roomName.Replace("U", string.Empty);;
            } else {
                roomName = roomName.Replace("D", string.Empty);;
            }
        }

        roomName = roomName.Replace("S", string.Empty);

        switch (roomName){
            case "R(Clone)":
                return onlyRightRooms;
            case "L(Clone)":
                return onlyLeftRooms;
            case "U(Clone)":
                return onlyTopRooms;
            case "D(Clone)":
                return onlyBottomRooms;
            default:
                return null;
        }
    }

    public bool ShouldInduceSpawnVariety(){
        if (rooms.Count < 4){
            int rand = Random.Range(1, 5);
            if (rand <= 5){
                return true;
            }
        }
        return false;
    }

    void UpdateMinimapCameraPosition(){
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        miniMapCamera.position = new Vector3(playerPos.x, playerPos.y, -1);
    }

    void DetermineEnemyRooms(){
        foreach (GameObject room in rooms){
            if (room == rooms[rooms.Count - 1]) continue;

            int rand = Random.Range(1, 100);
            if (rand <= enemySpawnChance){
                GameObject roomCollider = Instantiate(enemyRoom, room.transform.position, Quaternion.identity);
                roomCollider.transform.SetParent(room.transform);
            }
        }
    }

    void SpawnChests(){
        foreach (GameObject room in rooms){
            if (room == rooms[rooms.Count - 1]) continue;

            int rand = Random.Range(1, 100);
            if (rand <= chestSpawnChance && room.GetComponentInChildren<EnemyRoom>() == null){
                GameObject newChest = Instantiate(chest, room.transform.position, Quaternion.identity);
                newChest.transform.SetParent(room.transform);
            }
        }
    }
}



