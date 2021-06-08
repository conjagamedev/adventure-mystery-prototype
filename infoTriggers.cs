using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoTriggers : MonoBehaviour
{
    Collider playerCol;
    public Image infoBg;  
    public Text infoTxt;
    PlayerLocomotion playerScript; 
    public GameObject objectBg; 
    public GameObject objectTxt; 


    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLocomotion>();
        objectBg.active = false;
        objectTxt.active = false; 

    }

    void Update()
    {
        HandleFade();
    }

    void OnTriggerEnter(Collider playerCol)
    {
        if(playerScript.isActive)
        {
            objectBg.active = true; 
            objectTxt.active = true; 
            infoBg.color = new Color(infoBg.color.r, infoBg.color.g, infoBg.color.b, 0.8f);
            print("player is in trigger"); 
            infoTxt.text = "WASD to move - Space to jump - Q to roll"; 
            infoTxt.CrossFadeAlpha(1.0f, 1f, false);
            infoBg.CrossFadeAlpha(0.8f, 1f, false);
             
        }
    }

    void OnTriggerExit(Collider playerCol)
    {
        if(playerScript.isActive)
        {
            print("player left trigger"); 
            infoTxt.CrossFadeAlpha(0.0f, 1f, false);
            infoBg.CrossFadeAlpha(0.0f, 1f, false);
        }
    }

    void HandleFade()
    {
        if(playerScript.isActive)
        {
            if(infoTxt.color.a == 1.0f)
            {
                infoTxt.enabled = true; 
            }
            if(infoTxt.color.a == 0.0f)
            {
                infoTxt.enabled = false; 
                infoTxt.text = "";
            }
            if(infoBg.color.a == 0.8f)
            {
                infoBg.enabled = true; 
            }
            if(infoBg.color.a == 0.0f)
            {
                infoBg.enabled = false; 
            }
        }
    }
     

}
