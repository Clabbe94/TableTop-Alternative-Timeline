using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMove : UnitMove
{
    
    public GameObject combatOptions;
    public GameObject winScreen;
    public GameObject startMenu;
    public GameObject optionMenu;

    public bool movePressed = false;
    bool attackPressed = false;
   

    float timeBetweenSounds = 0.552f;

    //Audiophile
    public AudioSource shootingSound;
    public AudioSource walkSound;
    public AudioSource selectionSound;

    void Awake()
    {
        shootingSound = GetComponent<AudioSource>();
        
    }
    

    public LaserPointer pointer = new LaserPointer(); // Refer to the Laserpointer which is used by the player. 
    void Start()
    {

        init();
        //Debug.Log(oppLength.ToString());

        findFogTiles();
        hideOpponents();


        if (combatOptions == null)
        {
            combatOptions = GameObject.FindGameObjectWithTag("CombatOptions");
        }

    }




    public void chooseTile()
    {
        bool b = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (b)//&& !combatOptions.activeSelf)
        {
            Ray ray = Camera.main.ScreenPointToRay(pointer._endPoint);     //(OVRInput.GetLocalControllerPosition(OVRInput.Controller.Active));
            RaycastHit hit;
            if (Physics.Raycast(pointer._endPoint, -Vector3.up, out hit, 1))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (getObjectAboveTile(t).CompareTag("FriendlyUnit"))
                    {
                        //t.pointedTile = true (gul färg)
                        t.current = true;   
                        getObjectAboveTile(t).turn = true;

                        foreach (GameObject g in TurnManager.friendly) // Setting all other unit's  turn variable to false
                        {
                            g.GetComponent<PlayerMove>().turn = false;
                        }



                        
                    }
                }
            }
        }
    } 


    void LateUpdate()
    {

        // chooseTile();
        
       

        if (health < 1) // kills unit
        { 
            
            if (!getTargetTile(this.gameObject).dead) // Dead Variable solves the super reappearances of gameobject at its death.
            {
                getTargetTile(this.gameObject).dead = true;
                this.gameObject.SetActive(false);
            }
            
            //Destroy(this.gameObject); //If we dont destroy them, we're stuck with massive amount of instances

            if (this.gameObject.name == "EnemyCommander")
            {
                winScreen.SetActive(true);
                startMenu.SetActive(false);
                optionMenu.SetActive(false);
            }

        }
        else if (!turn)
        {
            findFogTiles();
            hideOpponents();
            return; // Do nothing because it's not your turn. 
        }
        else if (!moving && !attacking) //todo : change the logic
        {
            if (!movePressed && !attackPressed)
            {
                 //chooseTile();
                findFogTiles(); // TEST
                // choose unit before deciding a move
            }
                walkSound.Stop();

            //fog methods
            findFogTiles();
            hideOpponents();


            if (!combatOptions.activeSelf)
            {
                combatOptions.SetActive(true);
            }

            if (movePressed) // move commence
            {
                
                if (attackPressed) // exception handling
                {
                    removeSelectableTiles();
                    movePressed = false;
                    attackPoint = 5;
                    findAttackableTiles();
                    checkIfClicked();
                    
                }
                else
                {
                    findSelectableTiles();
                    checkIfClicked();
                }
                
                
            }
            else if (attackPressed) // attack commence
            {
                removeSelectableTiles();
                if (movePressed) // exception handling
                {
                    
                    attackPressed = false;
                    foreach (Tile tile in attackableTiles)
                    {
                        tile.attackable = false;
                    }

                    attackPressedReset();
                }
                else
                {
                    attackPoint = 5;
                    findAttackableTiles();
                    checkIfClicked();
                } 
    
            }
        }
        else
        {
            Move();
            if (combatOptions.activeSelf)
            {
                combatOptions.SetActive(false);
            }
            attacking = false;
            movePressedReset();
        }
    }


    
    /*
    public bool checkFloatTrigger ()
    {
        float frame1 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        float frame2;
        for (int i=0;i<2;i++)
        {
            frame2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        }
        
        if ((frame1 < 0.99f ) && (frame2 => 0.99f))
        {
            return true;
        }
        else
        {
            return false
        }
    }
    */

    public void checkIfClicked() // check if selected by hand-controller. 
    {

        //if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch)) // button 'A'
        //if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 1f) //primary index trigger on right hand
        //OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) this works too :D
        bool b = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (b)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(pointer._endPoint);    //(OVRInput.GetLocalControllerPosition(OVRInput.Controller.Active));
            RaycastHit hit;
            if(Physics.Raycast(pointer._endPoint, -Vector3.up, out hit, 1))
            {
                if(hit.collider.tag == "Tile")
                {                  
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)
                    {
                        if(movePressed)
                        {
                            moveToTile(t);
                            StartCoroutine(soundLoop());
                        } 
                    }
                    if (t.attackable && attackPressed)
                    {
                        attackPressedReset();
                        shootingSound.Play();
                        // a method that decrement health, make combat visual effect.

                        PlayerMove enemy = getObjectAboveTile(t);
                        enemy.takeDamage(attackPoint); 


                        
                        foreach (Tile s in attackableTiles)
                        {
                            s.attackable = false;

                        }
                        //TurnManager.endTurn();

                    }
                }
            }
        }
    }


    //Button Logic
    public void setMovePressed()
    {
        movePressed = true;
        attackPressedReset();
        selectionSound.Play();
    }

    public void movePressedReset()
    {
        movePressed = false;
    }

    public void setAttackPressed()
    {
        attackPressed = true;
        movePressedReset();
        findAttackableTiles();
        selectionSound.Play();

        if (inRange)
        {
            selectionSound.Play();
            //inRange = false;
        }
        
    }

    public void attackPressedReset()
    {
        attackPressed = false;
        attackableTiles.Clear();
    }

    //Combat methods
    //AudioMethod
    
   IEnumerator soundLoop()
    {
        while (moving)
        { //or for(;;){
            walkSound.Play();
            yield return new WaitForSeconds(timeBetweenSounds);
           
        }
    }


 

}
