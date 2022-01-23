using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocketUIController : MonoBehaviour
{
    [SerializeField] private RocketMonitor _monitor = default;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI _countdownText = default;
    [SerializeField] private TextMeshProUGUI _maxHeightText = default;
    [SerializeField] private TextMeshProUGUI _currHeightText = default;
    [SerializeField] private TextMeshProUGUI _currSpeed = default;

    [Header("Speed mark")]
    [SerializeField] private Image _markSpeed = default;
    [SerializeField] private Color _speedUpColor = Color.green;
    [SerializeField] private Color _slowDownColor = Color.red;

    [Space]
    [SerializeField] private GameObject _buttonStartCountdown = default;
    [SerializeField] private GameObject _countdownPanel = default;

    private bool _isReachedMaximumHeight;

    private void LateUpdate()
    {
        if (!_isReachedMaximumHeight)
            WriteMaxHeight();

        WriteCurrentHeight();
        WriteCurrentSpeed();
        ColoredMarkSpeed();
    }

    public void OnReachMaximumHeight(float height)
    {
        if (_maxHeightText == null)
            return;

        _maxHeightText.text = $"{height : 0#.##}";
        _isReachedMaximumHeight = true;
    }

    public void HideButton()
    {
        _buttonStartCountdown.SetActive(false);
    }

    public void HideCountdown()
    {
        _countdownPanel.SetActive(false);
    }

    public void WriteCountDown(string countdown)
    {
        if (_countdownText == null)
            return;

        _countdownPanel.SetActive(true);
        _countdownText.text = countdown;
    }
    
    private void WriteMaxHeight()
    {
        if (_maxHeightText == null)
            return;

        _maxHeightText.text = $"{_monitor.CurrentHeight : 0#.##}";
    }

    private void WriteCurrentHeight()
    {
        if (_currHeightText == null)
            return;

        _currHeightText.text = $"{_monitor.CurrentHeight : 0#.##}";
    }

    private void WriteCurrentSpeed()
    {
        if (_currSpeed == null)
            return;

        _currSpeed.text = $"{_monitor.Speed: 0#.##}";
    }

    private void ColoredMarkSpeed()
    {
        if (_markSpeed == null)
            return;

        _markSpeed.color = _monitor.IsAccelerating ? _speedUpColor : _slowDownColor;
    }

}
