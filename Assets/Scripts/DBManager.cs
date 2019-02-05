using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager {

    //users data
    public static string username;
    public static int money;
    public static int shops;
    public static int numTrucks;
    public static int truckSpeed;
    public static int truckCarryingCapacity;

    //account settings
    public static int music; //1=true 0=false
    public static int sfx; //1=true 0=false
    public static int volume;

    public static Camera gameplayCamera;

    public static bool LoggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
        money = 0;
        shops = 0;
        numTrucks = 0;
        truckSpeed = 0;
        truckCarryingCapacity = 0;
        music = 0;
        sfx = 0;
        volume = 0;
        gameplayCamera = null;
    }
}
