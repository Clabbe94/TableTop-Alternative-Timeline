using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommanderMove : UnitMove
{
    //Declaration

    [SerializeField]
    GameObject winScreen;

    void Start()
    {
        init();
        //Debug.Log(oppLength.ToString());
    }

    void Update()
    { 
        if (health < 1f) // kills unit
        {
            //getOpponentTile(this.gameObject).dead = true;
            this.gameObject.SetActive(false);


            winScreen.SetActive(true);
           
        }


        if (!turn)
        {
            return;
        }
    }
}
