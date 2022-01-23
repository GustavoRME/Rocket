using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private Launcher _launcherFirstStage = default;
    [SerializeField] private Launcher _launcherSecondStage = default;
    [SerializeField] private Decoupler _decoupler = default;
    [SerializeField] private Parachute _parachute = default;
    [SerializeField] private RocketMonitor _monitor = default;

    [Header("Rocket Settings")]

    [Header("Fly Stage Time")]
    [SerializeField] private float _timeFirstStage = 5.0f;
    [SerializeField] private float _timeSecondStage = 5.0f;

    [Space]
    [SerializeField] private float _waitTimeSecondStageIgnition;         //Time to wait to start the second stage
    [SerializeField] private float _waitTimeToDecoupler;                 //Time to wait for Decoupler first stage
    [SerializeField] private float _waitTimeToOpenParachute;

    private float _time;

    private bool _isFirstStageOn;
    private bool _isSecondStageOn;
    private bool _isDecoupled;
    private bool _isParachuteOpen;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        float timeElapsed = Time.time - _time;
        if(_isFirstStageOn)
        {
            //While the first stage is on, update the launcher. Even the first stage already been decoupled, need update the rotation
            _launcherFirstStage.UpdateMe();
            
            if(!_launcherFirstStage.IsStopEffects && timeElapsed > _timeFirstStage)
            {
                _launcherFirstStage.StopEffects();          //Stop any particle and sound
                
                //Update time
                _time = Time.time;
                timeElapsed = Time.time - _time;
            }

            //Check if can already decouple the first stage
            bool canDecouple = !_isDecoupled && _launcherFirstStage.IsStopEffects && timeElapsed > _waitTimeToDecoupler;
            if (canDecouple)
            {
                DecoupleFirstStage();
                _isDecoupled = true;

                //Update time
                _time = Time.time;
                timeElapsed = Time.time - _time;
            }

            //After decoupled check if can start the second stage ignition
            if(_isDecoupled && timeElapsed > _waitTimeSecondStageIgnition)
            {
                StartSecondStageIgnition();
                
                //Update time
                _time = Time.time;
                timeElapsed = Time.time - _time;
            }

            Debug.Log("Its updating the first stage");
        }

        if(_isSecondStageOn)
        {
            _launcherSecondStage.UpdateMe();

            //Stop effects when time over
            if(!_launcherSecondStage.IsStopEffects && timeElapsed > _timeSecondStage)
            {
                _launcherSecondStage.StopEffects();
                
                //Update time
                _time = Time.time;
                timeElapsed = Time.time - _time;
            }

            //Only Open Parachute when the rocket stopped its acceleration and when the time arrive
            if(!_launcherSecondStage.IsAccelerating && timeElapsed > _waitTimeToOpenParachute)
            {
                OpenParachute();
                _isSecondStageOn = false;
            }

            Debug.Log("Its updating the second stage");
            Debug.Log($"time Elpased {timeElapsed}");
        }

        //Only need update the parachute while wasn't slowed down the rocket
        if (_parachute.IsOpened && !_parachute.IsSlowedDown)
        {
            _parachute.UpdateMe();
        }

        _monitor.UpdateMonitor();
        
        if(_monitor.IsCloseFromFloor)
        {
            CloseParachute();
            enabled = false;
        }
    }

    public void StartIgnition()
    {
        StartFirstStageIgnition();
        _time = Time.deltaTime;
        enabled = true;
    }

    private void StartFirstStageIgnition()
    {
        _launcherFirstStage.Launch(_timeFirstStage);
        _isFirstStageOn = true;
        _isSecondStageOn = false;
    }

    private void StartSecondStageIgnition()
    {
        _launcherSecondStage.Launch(_timeSecondStage);
        _isFirstStageOn = false;
        _isSecondStageOn = true;
    }

    private void DecoupleFirstStage()
    {
        _decoupler.DecoupleStage();
    }

    private void OpenParachute()
    {
        _parachute.OpenParachute();
    }

    private void CloseParachute()
    {
        _parachute.CloseParachute();
    }
}
