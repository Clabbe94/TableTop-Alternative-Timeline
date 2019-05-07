using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> adjacencyAttackableList = new List<Tile>();

    //public bool dead = false;
    public bool currentFog = false;

    public bool attackable = false;
    //public bool walkable = true; Överväg starkt att ta bort det
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool dead = false;
    public bool fog = true;
    public bool pointedTile = false;
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    // For A*
    // Best case path finding.
    public float f = 0; // f = g+h
    public float g = 0; // Cost from parent to child?
    public float h = 0; // Cost from current tile to destination?




    void Start()
    {

    }
    
    void Update()
    {
        if(current) // Highlight selected
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        } else if (target) // Highlight available movement options
        {
            GetComponent<Renderer>().material.color = Color.green;
        } else if (selectable) // Highlight selected while moving area
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (attackable)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (pointedTile)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (fog)
        {
            GetComponent<Renderer>().material.color = Color.grey;
        }
        else // If nothing, white. 
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

    }

    public void reset() // reset tiles parameters
    {
        adjacencyList.Clear();
        adjacencyAttackableList.Clear();
        attackable = false;
        current = false;
        target = false;
        selectable = false;
        fog = true;
        pointedTile = false;
        

        visited = false; // Raycast -> tiles not chosen in order -> visited marks them as chosen/processed. Use by some method.
        parent = null;
        distance = 0;

        f = 0;
        g = 0; 
        h = 0;

}

    public void findNeighbours(Tile target)
    {
        reset();

        checkTile(new Vector3(0,0,0.8f), target);
        checkTile(new Vector3(0, 0, -0.8f), target);
        checkTile(new Vector3(0.8f, 0, 0), target);
        checkTile(new Vector3(-0.8f, 0, 0), target);
    }

    public void findAdjacencyTiles(Tile target) // Equivalent Fog MEthod
    {
        checkFogTile(new Vector3(0, 0, 0.8f), target);
        checkFogTile(new Vector3(0, 0, -0.8f), target);
        checkFogTile(new Vector3(0.8f, 0, 0), target);
        checkFogTile(new Vector3(-0.8f, 0, 0), target);
    }


    public void checkTile(Vector3 direction, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.2f,0.8f,0.2f); // Condition's radius
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position,Vector3.up,out hit,1) || (item == target)) // origin, direction ,   ,maxdistance
                { // (out)As a parameter modifier, which lets you pass an argument to a method by reference rather than by value.
                    adjacencyList.Add(tile);
                }
            }
        }

    }

    public List<Tile> adjacencyFogList;
    public void checkFogTile(Vector3 direction, Tile target)  // Equivalent Fog Method
    {
        Vector3 halfExtents = new Vector3(0.2f, 0.8f, 0.2f); // Condition's radius
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null)
            {
                adjacencyFogList.Add(tile);   
            }
        }

    }

}
