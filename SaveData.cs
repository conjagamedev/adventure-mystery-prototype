using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
   public float[] playerPosition;

   public SaveData (playerSave playerSave)
   {
       playerPosition = new float[3]; 
       playerPosition[0] = playerSave.transform.position.x; 
       playerPosition[1] = playerSave.transform.position.y; 
       playerPosition[2] = playerSave.transform.position.z;
   }
}
