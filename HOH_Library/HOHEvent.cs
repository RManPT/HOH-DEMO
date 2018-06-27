﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOH_Library
{
    public class HOHEvent : EventArgs
    {
        public string LogMsg { get; set; }
        public string UserMsg { get; set; }
        public int ExerciseTimer { get; set; }
        public string ExerciseName { get; set; }
        public string ProtocolState { get; set; }

        public static event EventHandler<HOHEvent> LogUpdated;
        public static event EventHandler<HOHEvent> UsrMsgUpdated;
        public static event EventHandler<HOHEvent> ExerciseTimerUpdated;
        public static event EventHandler<HOHEvent> ExerciseNameUpdated;
        public static event EventHandler<HOHEvent> ProtocolStateUpdated;

        public HOHEvent()
        {

        }

        protected virtual void OnLogUpdated(HOHEvent e)
        {
            LogUpdated?.Invoke(this, e);
        }

        protected virtual void OnUsrMsgUpdated(HOHEvent e)
        {
            UsrMsgUpdated?.Invoke(this, e);
        }
        protected virtual void OnExerciseNameUpdated(HOHEvent e)
        {
            ExerciseNameUpdated?.Invoke(this, e);
        }

        protected virtual void OnExerciseTimerUpdated(HOHEvent e)
        {
            ExerciseTimerUpdated?.Invoke(this, e);
        }
        protected virtual void OnProtocolStateUpdated(HOHEvent e)
        {
            ProtocolStateUpdated?.Invoke(this, e);
        }


        public void UpdateLogMsg(string strInformation)
        {
            OnLogUpdated(new HOHEvent() {LogMsg = strInformation});
        }

        public void UpdateUsrMsg(string strInformation)
        {
            OnUsrMsgUpdated(new HOHEvent() {UserMsg = strInformation});
        }

        public void UpdateExerciseTimer(int counter)
        {
            OnUsrMsgUpdated(new HOHEvent() {ExerciseTimer = counter});
        }
        public void UpdateExerciseName(string name)
        {
            OnExerciseNameUpdated(new HOHEvent() { ExerciseName = name });
        }
        public void UpdateProtocolState(string state)
        {
            OnProtocolStateUpdated(new HOHEvent() { ProtocolState = state });
        }
    }
}