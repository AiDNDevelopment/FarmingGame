using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
    //Time it tkes for the seed to mature
    public int daysToGrow;

    //How much of ccrop to yield
    public ItemData cropToYield; 
    
    //The seedling GameObject
    public GameObject seedling;

    [Header("Regrowable")]
    //Is the plant regrowable
    public bool regrowable;
    //Time taken before regrowing and yielding another crop
    public int daysToRegrow;
}
