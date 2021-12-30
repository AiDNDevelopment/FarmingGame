using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}

    [Header("Internal Clock")]//Dont let the internal clock run dry!!!!!!!
    [SerializeField]
    GameTimestamp timestamp;
    public float timeScale = 1.0f;

    [Header("Day and Night Cycle")]

    //The transform of the directional light in scene(Sun)    
    public Transform sunTransform;

    //List of objects to inform of changes to the time
    List<ITimeTracker> listeners = new List<ITimeTracker>();

    //If there is more than one instance running destroy it
    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            //set the static instance to this instance
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Initialise the time stamp
        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate(){
        while(true){
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }
    }
    //A tick of each in-game time
    public void Tick(){
        timestamp.UpdateClock();

        //Inform the listeners of the new timestate
        foreach(ITimeTracker listener in listeners){
            listener.ClockUpdate(timestamp);
        }

        updateSunMovement();
    }

    void updateSunMovement(){
        //Convert the current time to minutes
        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        //Subn moves 15 degrees in an hour
        //.25 degrees in a minute
        //at mindnight the angle of the sun should be -90
        float sunAngle = .25f * timeInMinutes - 90;

        //apply the angle to the directional light
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //Get the timestamp    
    public GameTimestamp GetGameTimestamp(){
        //Return a cloned instance
        return new GameTimestamp(timestamp);
    }

    //Handling Listeners
    public void RegisterTracker(ITimeTracker listener){
        listeners.Add(listener);
    }

    public void UnregisterTracker(ITimeTracker listener){
        listeners.Remove(listener);
    }

}
