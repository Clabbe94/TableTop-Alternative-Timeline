using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMove : UnitMove
{
    GameObject target;



    void Start()
    {
        init();
    }

    void Update()
    {
        if (!turn)
        {
            return;
        }
        if (!moving)
        {
            findNearestTarget();
            calculatePath();
            findSelectableTiles();
            actualTargetTile.target = true;

        }
        else
        {
            Move();
        }
    }

    void calculatePath()
    {
        Tile targetTile = getTargetTile(target);
        findPath(targetTile);
    }

    void findNearestTarget() // what it finds depends on the definition inside the clause.
    {
        GameObject[] targets =  GameObject.FindGameObjectsWithTag("Player"); // Specify what unit this function is to find using tag (tag might change later)

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            // Vector3.SqrMagnitude works too.

        if (d < distance) // Checks if the current target is the nearest.
            {
                distance = d;
                nearest = obj;

            }



        }


        target = nearest;

    }



}





