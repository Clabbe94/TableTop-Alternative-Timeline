using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : PlayerMove
{
    /* //How to access methods and variable in script belonging to gameobjects
     * GameObject go = GameObject.Find("somegameobjectname");
    ScriptB other = (ScriptB) go.GetComponent(typeof(ScriptB));
    other.DoSomething()*/


    public static GameObject[] friendly;  // Use these later to command them from the MENU
    public static GameObject[] opponent;

    private void Start()
    {
        friendly = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        opponent = GameObject.FindGameObjectsWithTag("OpponentUnit");
    }





    void Update()
    {
        
        chooseTile();
        foreach (GameObject g in friendly)
        {
            //todo: cast gameobject
            if (g.GetComponent<PlayerMove>().turn == true)
            {


            }


        }
    }

            /*
            static Dictionary<string, List<UnitMove>> units = new Dictionary<string, List<UnitMove>>(); // unit is a unit list for a team
            static Queue<string> turnKey = new Queue<string>(); // string is a tag that tells whose turn it is.
            public static Queue<UnitMove> turnTeam = new Queue<UnitMove>(); // The current team's UnitMove which govern the game behaviour. The queue count is the same as units that hasn't moved


            void Start ()
            {

            }



            void Update() // This is how it changes turn
            {
                if (turnTeam.Count == 0)
                {
                    initTeamTurnQueue(); 
                }
            }

            static void initTeamTurnQueue ()
            {
                List<UnitMove> teamList = units[turnKey.Peek()]; // Put a list of units which totally belongs to the right team due to tagging

                foreach (UnitMove unit in teamList)
                {
                    turnTeam.Enqueue(unit); // inserting at the end of the "list"
                }
                startTurn();
            }

            public static void startTurn()
            {
                if (turnTeam.Count > 0)
                {
                    turnTeam.Peek().beginTurn();



                    // When it's enemy's turn, for now they will basically do nothing. 
                    if (turnKey.Peek().Equals("Opponent"))
                    {
                        endTurn();
                    }
                }
            }

            public static void endTurn()
            {
                UnitMove unit = turnTeam.Dequeue();
                unit.endTurn();

                if(turnTeam.Count > 0 )
                {
                    startTurn();
                }
                else
                {
                    string team = turnKey.Dequeue();
                    turnKey.Enqueue(team);
                    initTeamTurnQueue();
                }
            }

            public static void addUnit(UnitMove unit)
            {
                List<UnitMove> list;
                if (!units.ContainsKey(unit.tag))
                {
                    list = new List<UnitMove>();
                    units[unit.tag] = list;

                    if (!turnKey.Contains(unit.tag))
                    {
                        turnKey.Enqueue(unit.tag);
                    }
                }
                else
                {
                    list = units[unit.tag];
                }
                list.Add(unit);
            }



        */



    
}
