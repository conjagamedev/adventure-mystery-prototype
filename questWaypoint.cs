using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class questWaypoint : MonoBehaviour
{
    public Image markerImg;
    public Transform target; 
    public Text questDistanceTxt; 
    public Text questInfoTxt;  
    Vector2 markerHeight; 

    pugScript pugScript; 
    talkToDumbass talkToDumbass;

    public GameObject quests; 

    Vector3 offset; 

    void Start()
    {
        quests.active = false; 
        pugScript = GameObject.Find("Pug").GetComponent<pugScript>();
        talkToDumbass = GameObject.Find("Dumbass").GetComponentInChildren<talkToDumbass>();
        target = this.transform;
    }
    void Update()
    {
        Logic();

        if(pugScript.isPickedUp && !talkToDumbass.inTalk )
        {
            offset = new Vector3(0,3.5f,0);
            target = GameObject.Find("Dumbass").GetComponent<Transform>();
            quests.active = true; 
            questInfoTxt.text = "Bring the dog to the stranger";
        }
        else 
        {
            quests.active = false; 
        }


    }

    void Logic() 
    {
        float minX = markerImg.GetPixelAdjustedRect().width / 2; 
        float maxX = Screen.width - minX; 
        
        float minY = markerImg.GetPixelAdjustedRect().height / 2; 
        float maxY = Screen.height - minY; 

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset) ;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        if(Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            //target behind player
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        markerImg.transform.position = pos;
        questDistanceTxt.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }


}

