using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//todo no Move

public class UnitMove : MonoBehaviour //todo no Move
{
    public bool turn = false;
    public Canvas healthBarCanvas;
    float halfHeight = 0.6f; // todo: adjust to good height when we work with collider and model later

    List<Tile> selectableTiles = new List<Tile>();
    public List<Tile> attackableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;
    Tile currentFogTile;
    Tile currentOpponentTile;
    GameObject[] opponents;


    //[Header("Unity Stuff")]
    public Image healthBar;

    
    //[HideInInspector]
    public float startHealth = 15;
    public float health;
    


    //combat
    public static int originAttackPoint = 3;
    public int attackPoint = originAttackPoint;


    //public static int startHealth = 15;
    //public int health = startHealth;
    public int attackRange = 5;
    public bool attacking = false;

    public int lineOfSight = 4;

    public bool moving = false;
    public static int originMove = 19; // Begynnelse movepoint
    public int move = originMove;      // Works in integer, count tiles not "distance"
    public float moveSpeed = 6;

    Vector3 velocity = new Vector3(); //Speed at which the piece moves
    Vector3 heading = new Vector3(); // Direction the piece is moving

    public Tile actualTargetTile;



    protected void init()
    {
        health = startHealth;
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        //halfHeight = GetComponent<Collider>().bounds.extents.y;

       // TurnManager.addUnit(this);

    }

    GameObject[] opp; //Needs to be declared before function in order to remember orginal objects

    public void hideOpponents()
    {

        if (opp == null) //Had to be declaired in the first function call
        { 
            opp = GameObject.FindGameObjectsWithTag("Opponent");
            //oppLength = opp.Length;
        }
        foreach (GameObject g in opp) //Sets Opponents to invisible if they are in fog and reveal the otherwise
        {

            if (getTargetTile(g).fog)
            {
                
                g.gameObject.SetActive(false);
            }

            if (!getTargetTile(g).fog && !getTargetTile(g).dead )
            {
                
                g.gameObject.SetActive(true);
            }
        }
    }

    public void getCurrentTile()
    {
        currentTile = getTargetTile(gameObject);
        currentTile.current = true;

    }

    public void getCurrentFogTile()
    {
        currentFogTile = getTargetTile(gameObject);
        currentFogTile.currentFog = true;

    }


    public Tile getTargetTile(GameObject target) // Unit checks what tile is under it
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1)) 
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

   public PlayerMove getObjectAboveTile (Tile tile)
   {
       RaycastHit hit;
       PlayerMove unit = null;

       if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
       {
           unit = hit.collider.GetComponent<PlayerMove>();
       }
       return unit;
   }

    public void computedAdjacencyLists(Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.findNeighbours(target);

        }
    }

    public void computedFogAdjacencyLists (Tile target) // Equivalent fog method
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.findAdjacencyTiles(target);

        }
    }





    
    List<Tile> fogTiles;

    public void findFogTiles()
    {
        getCurrentFogTile();
        computedFogAdjacencyLists(null); //Equivalent fog
        
        Queue<Tile> process = new Queue<Tile>();


        process.Enqueue(currentFogTile);

        currentFogTile.visited = true;
        // currentTile.parent

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            //selectableTiles.Add(t);

            //fogTiles.Add(t);

            t.fog = false;
            
            if (t.distance < lineOfSight) // Här kan man ändra move till line of sight.
            {
                foreach (Tile tile in t.adjacencyFogList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance; // count in number of tiles instead of "distance"
                        process.Enqueue(tile);
                       
                    }
                }
            }
        }

    }









    public void  findSelectableTiles()
    {
        computedAdjacencyLists(null);
        getCurrentTile();
        Queue<Tile> process = new Queue<Tile>();
        

        process.Enqueue(currentTile);
    
        currentTile.visited = true;
        //currentTile.parent 

        while(process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);

            t.selectable = true;
             
           
            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance; // count in number of tiles instead of "distance"
                        process.Enqueue(tile);
                    }
                }
            }
            

        }

    }

    public bool inRange = false;

    public void findAttackableTiles()
    {
        attackableTiles.Clear();
        opponents = GameObject.FindGameObjectsWithTag("Opponent");
        foreach (GameObject t in opponents)
        {
            Tile s = getTargetTile(t);
            Tile claudioCurrentTile = getTargetTile(gameObject);
            float tol = attackRange * 0.8f + 0.1f*0.8f; // tol = tolerans gräns

            float sumStep = Mathf.Abs(claudioCurrentTile.transform.position.x - s.transform.position.x) + Mathf.Abs(claudioCurrentTile.transform.position.z - s.transform.position.z) ;
            if (sumStep <= tol) // check if within attackrange.
            {
                s.attackable = true;
                s.fog = false; // if attackable, its seen.
                attackableTiles.Add(s);
                inRange = true;
            }
            else
            {
                inRange = false;
            }
        }
        
    }

    public void moveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        List<Tile> temp = new List<Tile>() ;
        while (next != null)
        {
            temp.Add(next);

            path.Push(next);
            next = next.parent;
        }    
    }

    public void Move() // move is already a variable, so has to take big alphabet, move() -> Move() (the code complained) 
    {
        
        //Vector3 hello = transform.forward;// direction before turning
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            Vector3 tempHeading = new Vector3(0,0,1f);
            RaycastHit hit;
           
            
            if (Vector3.Distance(transform.position, target) >= 0.05f && (!Physics.Raycast(this.gameObject.transform.position, Vector3.up, out hit, 1)))
            {
                calculateHeading(target);
                setHorizontalVelocity();

                transform.forward = heading;
                //todo fix rotation here
                

                //transform.LookAt(new Vector3(0, 0,99999f);  // Fixes Rotation when moving //99999f
                transform.LookAt(new Vector3(transform.position.x,transform.position.y,transform.position.z+5f)); // look forward always


                //transform.Rotate(0, Mathf.Acos(Vector3.Dot(tempHeading, heading)), 0); // Camera follow the direction it's heading to.
                transform.position += velocity * Time.deltaTime; // make it move

                

            }
            else // Tile center reached
            {
                transform.position = target;
                path.Pop(); // remove top "item" from stack
            }
            
        }
        else //after you moved
        {
            findFogTiles();
            hideOpponents();

            removeSelectableTiles();
            moving = false;

            
            //TurnManager.endTurn(); // ends the turn for the current unit
        }
    }

    protected void removeSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.reset();
        }

        selectableTiles.Clear();
    }

    void calculateHeading(Vector3 target )
    {
        heading = target - transform.position;
        heading.Normalize();

    }

    void setHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Tile findLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);


        return lowest;
    }

    protected Tile findEndTile (Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null) // Creates a path from targets parent to the tile next to our target,all the way back to the start (current top)
        {
            tempPath.Push(next);
            next = next.parent;
            
        }

        if (tempPath.Count <= move) // Check if target is within range.
        {
            return t.parent;
        }

        Tile endTile = null;

        for (int i = 0; i<= move; i++)
        {
            endTile = tempPath.Pop();   
        }
        RaycastHit hit;
        if (Physics.Raycast(next.transform.position, Vector3.up, out hit, 1))
        {
            return endTile;
        }
        else
        {
            return endTile.parent;
        }
    }




    protected void findPath(Tile target)
    {
        computedAdjacencyLists(target);
        getCurrentTile();

        //instatiate
        List<Tile> openList = new List<Tile>(); // List of tiles that have not been processed yet.
        List<Tile> closedList = new List<Tile>(); // List of processed tiles.
        //

        openList.Add(currentTile);
        //currentTile.parent = ?;

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h; // in this case, g = 0 but f = g+h

        while (openList.Count>0) // Sort according to f.
        {
            Tile t = findLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = findEndTile(t);
                moveToTile(actualTargetTile);
                return;

            }

            foreach (Tile tile in t.adjacencyList) // Loop through neighbouring tiles
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g) // if shorter path -> new parent tile.
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }

                }
                else
                {
                    tile.parent = t; // if we're here, it means this is the first time the method is processing the Tile.

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position); // distance to beginning
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position); // estimated distance from current to end tile.
                    tile.f = tile.g + tile.h; //Distance between start and end.

                    openList.Add(tile);


                }


                
            }

        }


        // todo: What do you do if there is no path to the target tile?
        Debug.Log("path not found");





    }

    public void beginTurn()
    {
        turn = true;
    }

    public void endTurn()
    {
        turn = false;
    }

    //Combat methods 
    public void takeDamage(int damage)
    {
        Tile t = getTargetTile(this.gameObject);
        t.attackable = false; // When taking damage, tile under reset its color.

        health -= damage;
        healthBar.fillAmount = health / startHealth;
    }

}
