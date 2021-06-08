using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class openingScene : MonoBehaviour
{
    public GameObject player; 
    public Animator playerAnim;
    PlayerLocomotion playerScript; 
    public Text uiText;  

    public GameObject cutsceneCam; 
    public GameObject playerCam; 
    GameObject trigger; 
    
    void Start()
    { 
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLocomotion>();
        trigger = GameObject.Find("WASD_Trig"); 
        uiText.color = new Color(uiText.color.r,uiText.color.g,uiText.color.b, 0f);

        if(!playerScript.isActive)
        {
            playerCam.active = false;
            playerAnim.Play("Sleeping");
            StartCoroutine(Wait8Sec());
        }
        else
        {
            gameObject.active = false; 
        }
    }
    

    IEnumerator Wait8Sec()
    {
        trigger.active = false; 
        yield return new WaitForSeconds(7.0f);
        playerAnim.Play("Standing Up"); 
        yield return new WaitForSeconds(1.0f);
        gameObject.GetComponent<FadeCamera>().FadeOut(2.5f);
        yield return new WaitForSeconds(2.5f);
        player.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        playerCam.active = true; 
        uiText.text = "WASD to move - Space to jump - Q to roll";
        cutsceneCam.active = false;
        playerScript.isActive = true; 
        playerAnim.SetBool("isActive", true);  
        trigger.active = true; 
        uiText.color = new Color(uiText.color.r,uiText.color.g,uiText.color.b, 1f); 

         
    }


}
