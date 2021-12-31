using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{  
    //The Item information the gameobject is supposed to represent
   public ItemData item;

   public void Pickup(){
       //Set the players inventory to the item
       InventoryManager.Instance.equippedItem = item;
       //Update the changes in the scene
       InventoryManager.Instance.RenderHand();
       //Destroy this instance so no multiple copies
       Destroy(gameObject);
   }
}
