using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pugBroughtBackTrigger : MonoBehaviour
{
    [SerializeField]
    Collider pugCollider;
    public bool atHome;  

   void OnTriggerEnter(Collider other)
   {
       if(other == pugCollider)
        atHome = true; 
   }

   void OnTriggerExit(Collider other)
   {
       if(other == pugCollider)
        atHome = false; 
   }
}
