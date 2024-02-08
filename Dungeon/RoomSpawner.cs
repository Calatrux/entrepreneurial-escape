using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;

	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;
	public bool invincible = false;

	public float waitTime = 100f;
    GameObject room;
    GameObject grid;

	void Start(){
        grid = GameObject.FindGameObjectWithTag("Grid");
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		Invoke("Spawn", 0.05f);
	}


	void Spawn(){
		if(spawned == false && templates.currentRooms < templates.maxRooms){
			if(openingDirection == 1){
				room = SpawnRoom(templates.bottomRooms);
			} else if(openingDirection == 2){
				room = SpawnRoom(templates.topRooms);
			} else if(openingDirection == 3){
				room = SpawnRoom(templates.leftRooms);
			} else if(openingDirection == 4){
				room = SpawnRoom(templates.rightRooms);
			}
			spawned = true;

            room.transform.SetParent(grid.transform);
			templates.rooms.Add(room);
			templates.currentRooms++;
		}
	}

	GameObject SpawnRoom(GameObject[] rooms){
		GameObject[] shuffledRooms = (GameObject[])rooms.Clone();

    	// Fisher-Yates shuffle
		for (int i = shuffledRooms.Length - 1; i > 0; i--) {
			int rand = Random.Range(0, i + 1);
			GameObject temp = shuffledRooms[i];
			shuffledRooms[i] = shuffledRooms[rand];
			shuffledRooms[rand] = temp;
		}

		rooms = shuffledRooms;

		if (templates.ShouldInduceSpawnVariety()){
			for (int i = 0; i < 100; i++){
				rand = Random.Range(0, rooms.Length);
				GameObject specificRoom = rooms[rand].gameObject;
				if (specificRoom.name.Length == 2 && !specificRoom.name.Contains("S")){
					break;
				}
			}
		}else{
			rand = Random.Range(0, rooms.Length - 1);
		}
		room = Instantiate(rooms[rand], transform.position, rooms[rand].transform.rotation);
		return room;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Spawnpoint")){
			spawned = true;
        }
    }

}