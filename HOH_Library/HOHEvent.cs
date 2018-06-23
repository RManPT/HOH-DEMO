using System;
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

        public static event EventHandler<HOHEvent> LogUpdate;
        public static event EventHandler<HOHEvent> UsrMsgUpdate;

        public HOHEvent()
        {

        }

        protected virtual void OnLogUpdated(HOHEvent e)
        {
            LogUpdate?.Invoke(this, e);
        }
        protected virtual void OnUsrMsgUpdated(HOHEvent e)
        {
            UsrMsgUpdate?.Invoke(this, e);
        }


        public void UpdateLogMsg(string strInformation)
        {
            OnLogUpdated(new HOHEvent() { LogMsg = strInformation });
        }

        public void UpdateUsrMsg(string strInformation)
        {
            OnUsrMsgUpdated(new HOHEvent() { UserMsg = strInformation });
        }

    }
}
