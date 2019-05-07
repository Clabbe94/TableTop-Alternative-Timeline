using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject startMenu;




    public void optionPress()
    {
        startMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
}
