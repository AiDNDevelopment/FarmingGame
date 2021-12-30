using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public enum Season{
        Spring, 
        Summer, 
        Fall, 
        Winter
    }

    public enum DayOfTheWeek{
        Sunday, 
        Monday, 
        Tuesday, 
        Wednesday, 
        Thursday, 
        Friday, 
        Saturday
    }

    public Season season;

    public int year; 
    public int day;
    public int hour; 
    public int minute; 


    //Constructor to setup the class
    public GameTimestamp(int year, Season season, int day, int hour, int minute){
        this.year = year;
        this.season = season; 
        this.day = day; 
        this.hour = hour; 
        this.minute = minute;
    }

    //Creating a new instance of gametimestamp from another existing one
    public GameTimestamp(GameTimestamp timestamp){
        this.year = timestamp.year;
        this.season = timestamp.season; 
        this.day = timestamp.day; 
        this.hour = timestamp.hour; 
        this.minute = timestamp.minute;
    }

    //Incremeants time by 1 minute

    public void UpdateClock(){
        minute++;

        //60 minutes in an hour
        if(minute >= 60){

            //reset minutes and add to hours
            minute = 0;
            hour++;
        }

        //24 hours in a day
        if(hour >= 24){

            //reset hours
            hour = 0;
            day++; 
        }

        //30 days in a season
        if(day >= 30){
            day = 1;

            //if season is winter and we are at day 30, roll that shit around to spring so we can start again
            if(season == Season.Winter){
                season = Season.Spring;
                year++;
            }else{
                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek(){

        //convert the total time passed into days
        int daysPassed = YearsToDays(year) + SeaonsToDays(season) + day;

        //remainder after dividing days passed by 7
        int dayIndex = daysPassed % 7;

        //Cast into day of the week
        return (DayOfTheWeek)dayIndex;
    }

    //Convert hours to mintes
    public static int HoursToMinutes(int hour){
        //60 minutes = 1hour
        return hour * 60;
    }

    //convert days to Hours    
    public static int DaysToHours(int days){
        //24 Hours in a day
        return days * 24;
    }

    //Convert seasons to days
    public static int SeaonsToDays(Season season){

        //30 days = 1 season
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    //Years to days
    public static int YearsToDays(int years){
        return years * 4 * 30;
    }


    //Calculate the difference between the two timestamps
    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2){

        //Convert the timestamp to hours
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeaonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeaonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;
        int difference = timestamp2Hours - timestamp1Hours;
        
        //return the difference between the two timestamps
        return Mathf.Abs(difference);
    }
}