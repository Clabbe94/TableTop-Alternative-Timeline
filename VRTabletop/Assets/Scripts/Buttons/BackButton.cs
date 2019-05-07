using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject startMenu;




    public void optionPress()
    {
        startMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}