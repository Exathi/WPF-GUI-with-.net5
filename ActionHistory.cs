using System;
using System.Collections.ObjectModel;

namespace GUI_Playground
{
    /// <summary>
    /// Object to log all button view actions
    /// </summary>
    class ActionHistory
    {
        public ActionHistory(DateTime dateTime, string description)
        {
            Time = dateTime;
            Description = description;
        }
        public DateTime Time { get; set; }
        public string Description { get; set; }
    }
}
