using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpaceStationDoorHandler : MonoBehaviour
{   
  
    [SerializeField]
    private List<StationDoor> Doors;

    public StationDoor GetDoor(int index) 
    {
        return Doors[index];
    }

    public void OpenDoor(int index)
    {
        if (Doors[index].openRatio == 0) 
        { 
            Doors[index].OpenDoor(this);
        }
    }
    public void CloseDoor(int index) 
    {
        if (Doors[index].openRatio == 1)
        {
            Doors[index].CloseDoor(this);
        }
    }
}
