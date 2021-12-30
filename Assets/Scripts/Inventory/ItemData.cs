using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string description;

    //Icon to be displayed in ui
    public Sprite thumbnail;

    //Gameobject to be shown
    public GameObject gameModel;
}
