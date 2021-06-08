using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSave : MonoBehaviour
{
    public Vector3 playerPos; 

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();

        playerPos.x = data.playerPosition[0];
        playerPos.y = data.playerPosition[1];
        playerPos.z = data.playerPosition[2];
        CharacterController playerController = GetComponent<CharacterController>();
        playerController.enabled = false; 
        this.transform.position = playerPos;
        playerController.enabled = true; 
    }
}
