using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNMMapplication
{
    [Serializable]
    class PreviousSessionState
    {
        public PreviousSessionState(MM_MK_Collection currentCollection)
        {
            TheDate = DateTime.Now;
            foreach (var unit in currentCollection.TheCollection)
            {
                string state = "";
                if (unit.IsOnline)
                    state = "Был в сети.";
                else
                {
                    state = "Не в сети.";
                    if (unit.LastDateOnline != null)
                        state += $" Последняя сессия {unit.LastDateOnline}";
                }
                TitleAndState.Add(unit.Title, state);
            }
        }
        public DateTime TheDate { get; set; }
        public Dictionary<string, string> TitleAndState { get; set; }
    }
}
