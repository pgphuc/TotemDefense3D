using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public int territoryID;
    public List<TerritoryGrid> gridsList = new List<TerritoryGrid>();
    public TerritoryState state;
    
    
    
    
    public void ChangeState(TerritoryState newState)
    {
        state = newState;
        if (state == TerritoryState.Unlocked)
        {
            foreach (TerritoryGrid grid in gridsList)
            {
                grid.GetComponent<Renderer>().material = grid.territoryMaterials[(int)state];
            }
        }
    }
 
}
