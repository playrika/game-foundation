using System;
using Playrika.GameFoundation.Common;

namespace Playrika.GameFoundation.Time
{
    public class Timer : StatefulObject<TimerState>
    {
        public event Action<TimeSpan> changed;

        public event Action oneSecondPassed;

        public event Action<float> progressChanged;

        public TimeSpan timeTotal { get; }

        public TimeSpan timeLeft { get; private set; }

        public bool isUnscaled { get; }

        public float progress { get; private set; }

        private float _oneSecondPassedChecker;


        public Timer(float intervalInSeconds, bool isUnscaled = true)
        {
            timeTotal = timeLeft = TimeSpan.FromSeconds(intervalInSeconds);
            this.isUnscaled = isUnscaled;
        }

        public Timer(TimeSpan interval, bool isUnscaled = true)
        {
            timeTotal = timeLeft = interval;
            this.isUnscaled = isUnscaled;
        }


        public void Start()
        {
            if (currentState != TimerState.Scheduled)
                return;

            SetState(TimerState.Started);
        }

        public void Pause(bool value)
        {
            if (currentState == TimerState.Started && value)
                SetState(TimerState.Paused);
            else if ( currentState == TimerState.Paused && !value)
                SetState(TimerState.Started);
        }

        public void Cancel()
        {
            if (currentState == TimerState.Canceled || currentState == TimerState.Completed)
                return;

            SetState(TimerState.Canceled);
        }


        public void Add(TimeSpan timeSpan)
        {
            if (currentState == TimerState.Canceled || currentState == TimerState.Completed)
                return;

            timeLeft = timeLeft.Add(timeSpan);
        }

        public void Add(float seconds)
        {
            Add(TimeSpan.FromSeconds(seconds));
        }


        public void Subtract(TimeSpan timeSpan)
        {
            if (currentState == TimerState.Canceled || currentState == TimerState.Completed)
                return;

            timeLeft = timeLeft.Subtract(timeSpan);
        }

        public void Subtract(float seconds)
        {
            Subtract(TimeSpan.FromSeconds(seconds));
        }


        public void Update(float deltaTime)
        {
            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(deltaTime));
            if (timeLeft.TotalSeconds < 0)
                timeLeft = TimeSpan.Zero;

            changed?.Invoke(timeLeft);

            progress = (float)(timeTotal.Subtract(timeLeft).TotalSeconds / timeTotal.TotalSeconds);
            progressChanged?.Invoke(progress);

            _oneSecondPassedChecker += deltaTime;
            if (_oneSecondPassedChecker >= 1f)
            {
                _oneSecondPassedChecker = 0f;
                oneSecondPassed?.Invoke();
            }

            if (timeLeft.TotalSeconds > 0)
                return;

            SetState(TimerState.Completed);
        }
    }
}