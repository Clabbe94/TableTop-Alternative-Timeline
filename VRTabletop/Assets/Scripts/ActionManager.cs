using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{ // Delete me
    public bool moveButtonPressed = false;
    public bool attackPressed = false;

    public void setMoveButtonPressed()
    {
        moveButtonPressed = true;
    }

    public void moveButtonPressedReset()
    {
        moveButtonPressed = false;
    }

    public void setAttackButtonPressed()
    {
        attackPressed = true;
    }

    public void attackButtonPressedReset()
    {
        attackPressed = false;
    }
}
