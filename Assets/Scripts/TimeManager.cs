using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;

    public float slowMotionScale = 0.25f;
    private bool timeStopped = false;

    //Pause time records
    private float pauseTime = 0;
    private float currentPauseStart = 0;

	// Use this for initialization
	void Start ()
    {
        #region Singleton
        if (instance != null)
        {
            Destroy(instance.gameObject);

        }

        instance = this;
        #endregion
    }

    /// <summary>
    /// The time in seconds the game has been running not including paused time;
    /// </summary>
    /// <returns></returns>
    public float getTime()
    {
        return Time.time - pauseTime;
    }

    public void normalTime()
    {
        stopTime(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 1 * 0.02f; //Scale physics time 0.02f is default value so times it by that to remain to the same scale as time
    }

    public void slowTime()
    {
        stopTime(false);
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = slowMotionScale * 0.02f; //Scale physics time 0.02f is default value so times it by that to remain to the same scale as time
    }
    
    #region Getters & Setters

    //SETTERS
    public void stopTime(bool value)
    {
        if (value)
        {
            normalTime();
            currentPauseStart = Time.time;
        }
        //Only add to pause time if the game was actually paused
        else if(timeStopped)
        {
            pauseTime += Time.time - currentPauseStart;
        }

        timeStopped = value;
    }

    //GETTERS
    public bool isTimeStopped()
    {
        return timeStopped;
    }

    #endregion
}
