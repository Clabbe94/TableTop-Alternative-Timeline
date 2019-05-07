using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{ 
    //Menus
    public GameObject startMenu;


    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch)) // Temporary startmenu activation button
        {
            startMenu.SetActive(true);

        }
    }



    

   



    


}
