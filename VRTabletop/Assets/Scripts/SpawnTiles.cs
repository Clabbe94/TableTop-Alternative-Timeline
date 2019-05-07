using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    [SerializeField]
    public GameObject Tile;
    public void spawnTiles()
    {
        GameObject[,] Tiles = new GameObject[20,20];
        //List<GameObject> Tiles = new List<GameObject>();
        Vector3 spawnPosition = new Vector3(-0.4f, 4.1f, -0.4f);
        Quaternion spawnRotation = Quaternion.identity;
        for (int j = 0; j < 20; j++)
        {
            spawnPosition.x += 0.8f;
            spawnPosition.z = -0.4f;
            for (int i = 0; i < 20; i++)
            {
                spawnPosition.z += 0.8f;
                Tiles[j,i] = Instantiate(Tile, spawnPosition, spawnRotation);
                //Tile.name = "Tile" + j + "" + i;
            }
        }
        Tiles[5, 5].GetComponent<Renderer>().material.color = Color.yellow;
        //Tile55(Clone).GetComponent<MeshRenderer>().material = Material1;
    }
}