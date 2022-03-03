using System;
using UnityEngine;

namespace DefaultNamespace.Util
{
    public class Countdown
    {
        private float startTime;
        private float currentTime;
        private float endTime;
        private bool isRunning;

        public Countdown(float startTime)
        {
            this.isRunning = false;
            this.startTime = startTime;
            this.currentTime = (int) this.startTime;
        }

        public float CurrentTime
        {
            get => (int) currentTime; //konvertiere float in int für ganze Zahlen!
        }

        public float StartTime
        {
            get => startTime;
        }

        public void Run()
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
            }
            
            if (this.currentTime > 0 && this.isRunning)
            {
                this.currentTime -= Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }

        public void Stop()
        {
            this.isRunning = false;
        }
    }
}