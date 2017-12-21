using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

public class Map {

	[SyncVar]
	public char[,] cases;

	public int sizeX;
	public int sizeZ;
	public Vector3 spawnPosP1;
	public Vector3 spawnPosP2;

	public Map(int sizeX, int sizeZ) {
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		this.cases = new char[sizeX, sizeZ];
		for(int i = 0; i < sizeX; i++) {
			for(int k = 0; k < sizeZ; k++) {
				this.cases[i, k] = ' ';
			}
		}
	}

	public void SetCase(int x, int z, char type) {
		this.cases [x, z] = type;
		if (type.Equals ('A')) {
			spawnPosP1 = new Vector3 (x, 2, z);
		} else if (type.Equals ('B')) {
			spawnPosP2 = new Vector3 (x, 2, z);
		}
	}

	public char GetCase(int x, int z) {
		return cases [x, z];
	}

	public bool IsEmpty(int x, int z) {
		return cases [x, z]==' ';
	}
}

//-----------------------------------------------------------------------
//-----------------------------------------------------------------------

public class MapGeneration : NetworkBehaviour {

	public static Map currentMap;
	public GameObject wallPrefab;
	public GameObject boxPrefab;
	public GameObject groundPrefab;
	public GameObject spawnPoint;

	public void CreateMap(string filePath) {
		string path = Application.streamingAssetsPath + "/"+ filePath;
		try{
			StreamReader reader = new StreamReader(path);  
			bool first = true;
			int k = 0;
			while(!reader.EndOfStream){
				string line = reader.ReadLine();
				if (first) {
					first = false;
					string[] numbers = line.Split(';');
					int sizeX = int.Parse(numbers[0]);
					int sizeZ = int.Parse(numbers[1]);
					currentMap = new Map(sizeX, sizeZ);
					k = sizeZ-1;
				} else {
					string[] symbols = line.Split(';');
					int i = 0;
					foreach (string symbol in symbols) {
						if (symbol.Equals("")){
							currentMap.SetCase(i, k, ' ');
						} else {
							currentMap.SetCase(i, k, symbol[0]);
						}
						i++;
					}
					k--;
				}
			}
			reader.Close();
		} catch (Exception e) {
			Debug.LogWarning ("Cannot load the map !\n"+e.Message);
		}
	}

	public void CreateStructure() {

		//Create SpawnPoints
		Instantiate (spawnPoint, currentMap.spawnPosP1, this.transform.rotation);
		Instantiate (spawnPoint, currentMap.spawnPosP2, this.transform.rotation);

		//Create Ground
		GameObject ground = (GameObject) Instantiate (groundPrefab, new Vector3 ((currentMap.sizeX-1f)/2, 0, (currentMap.sizeZ-1f)/2), this.transform.rotation);
		ground.transform.localScale += new Vector3(currentMap.sizeX-1f, -0.9f, currentMap.sizeZ-1f);

		//Create Walls and Boxes
		for(int i = 0; i < currentMap.sizeX; i++) {
			for(int k = 0; k < currentMap.sizeZ; k++) {
				if (currentMap.GetCase(i, k).Equals('X')){
					Instantiate (wallPrefab, new Vector3 (i, wallPrefab.transform.localScale.y/2, k), this.transform.rotation);
				} else if (currentMap.GetCase(i, k).Equals('O')) {
					Instantiate (boxPrefab, new Vector3 (i, boxPrefab.transform.localScale.y / 2, k), this.transform.rotation);
				}
			}
		}
	}

	public Vector3 GetMapPosition() {
		//Return the map position
		return (new Vector3 ((currentMap.sizeX - 1f) / 2, 0, (currentMap.sizeZ - 1f) / 2));
	}
}
