using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    string name;

    public abstract void pickupItem(PlayerMovement pm);
    public abstract void dropItem(PlayerMovement pm);
}
