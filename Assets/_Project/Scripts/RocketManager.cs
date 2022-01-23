using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
    [SerializeField] private RocketController _rocketController = default;
    [SerializeField] private RocketUIController _rocketUIController = default;

    [Space]
    [SerializeField] private int _countdown = 5;

    private Coroutine _countdownCoroutine;

    private void OnDestroy()
    {
        StopCoroutine(_countdownCoroutine);
    }

    public void StartCountdown()
    {
        _rocketUIController.HideButton();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        int time = _countdown;
        while (time > -1)
        {
            _rocketUIController.WriteCountDown(time.ToString());
            time--;
            yield return new WaitForSeconds(1f);
        }

        _rocketUIController.HideCountdown();
        _rocketController.StartIgnition();
    }
}
