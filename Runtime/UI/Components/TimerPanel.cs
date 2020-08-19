using System;
using UnityEngine;
using TMPro;
using Playrika.GameFoundation.Time;

namespace Playrika.GameFoundation.UI.Components
{
    public class TimerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private TimeFormat _timeFormat;

        private Timer _timer;

        private string _timeFormatString;


        public TimerPanel Initialize(Timer timer)
        {
            _timer = timer;
            _timer.stateChanged += OnTimerStateChanged;
            OnTimerStateChanged(_timer.currentState);
            return this;
        }

        public TimerPanel SetTimeFormat(TimeFormat value)
        {
            _timeFormat = value;
            return this;
        }


        private void Awake()
        {
            _timeFormatString = _timeFormat.ConvertToStringFormat();
        }

        private void OnDestroy()
        {
            if (_timer == null)
                return;

            _timer.stateChanged -= OnTimerStateChanged;
            _timer.changed -= OnTimerChange;
        }

        private void OnTimerStateChanged(TimerState state)
        {
            switch (state)
            {
                case TimerState.Scheduled:
                    OnTimerChange(_timer.timeLeft);
                    break;
                case TimerState.Started:
                    _timer.changed += OnTimerChange;
                    break;
                case TimerState.Completed:
                case TimerState.Canceled:
                    _timer.changed -= OnTimerChange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnTimerChange(TimeSpan timeLeft)
        {
            _timerText.text = timeLeft.ToString(_timeFormatString);
        }
    }
}