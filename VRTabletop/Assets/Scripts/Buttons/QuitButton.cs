using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void quitGame()
    {
        //Application.Quit(); //behöver eventuellt text? tona ut? .. .. ..
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
