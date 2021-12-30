using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlayerController playerController;

    //the land the player is currently selecting
    Land selectedLand = null;



    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down,out hit, 1)){
            OnInteractibleHit(hit);
        }
    }

    //Handles what happenswhen the raycast hits somthing
    void OnInteractibleHit(RaycastHit hit){
        Collider other = hit.collider;
        
        if(other.tag == "Land"){
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        if(selectedLand != null){
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    //handles the slection process of the land
    void SelectLand(Land land){

        //Set the previously selected land to false(if any)
        if(selectedLand != null){
            selectedLand.Select(false);
        }
        //Set the new selected land to the land we are stood on
        selectedLand = land;
        land.Select(true);
    }

    //Triggered when player presses tool button key
    public void Interact(){
        //check to see if player is on interactable land
        if(selectedLand != null){
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land");
    }
   
    
}
