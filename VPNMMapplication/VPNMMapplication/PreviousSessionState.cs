using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace VPNMMapplication
{
    [Serializable]
    public class SessionsArray
    {
        public List<PreviousSessionStatuses> SessionsLog { get; set; }
        public SessionsArray()
        {
        }

        public SessionsArray(MM_MK_Collection currentDisplayedCol)
        {
            try
            {
                SessionsArray array = StateSerialiser.DeSerializeList();
                if (array == null)
                {
                    SessionsLog = new List<PreviousSessionStatuses>();
                    PreviousSessionStatuses status = new PreviousSessionStatuses();
                    status.MakeStates(currentDisplayedCol);
                    SessionsLog.Add(status);
                    StateSerialiser.Serialize(this);
                }
                else
                {
                    SessionsLog = array.SessionsLog;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "В конструкторе");
            }
        }
    }


    [Serializable]
    public class PreviousSessionStatuses
    {
        public List<PreviousSessionState> Statuses { get; set; } = new List<PreviousSessionState>();
        public PreviousSessionStatuses()
        {
        }

        public void MakeStates(MM_MK_Collection currentCollection)
        {
            try
            {
                Statuses.Clear();
                foreach (var unit in currentCollection.TheCollection)
                {
                    PreviousSessionState state = new PreviousSessionState();
                    state.TheDate = DateTime.Now;
                    string status = "";
                    if (unit.IsOnline)
                        status = "Был в сети.";
                    else
                    {
                        status = "Не в сети.";
                        if (!string.IsNullOrWhiteSpace(unit.LastDateOnline))
                            status += $" Последняя сессия {unit.LastDateOnline}";
                    }
                    state.TitleAndState = unit.Title + " " + status;
                    Statuses.Add(state);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Создание коллекции статусов");
            }
        }

    }

    [Serializable]
    public class PreviousSessionState
    {
        public DateTime TheDate { get; set; } = new DateTime();
        public string TitleAndState { get; set; } = "";
    }

    public static class StateSerialiser
    {
        //Сериализует данные по поледним статусам в бин файл
        public static void Serialize(SessionsArray statusesList)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.OpenOrCreate,
                    FileAccess.Write, FileShare.None))
                {
                    binary.Serialize(stream, statusesList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сериализации лога");
            }

        }

        public static SessionsArray DeSerializeList()
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                SessionsArray statuses;
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Open,
                    FileAccess.Read, FileShare.None))
                {

                    statuses = (SessionsArray)binary.Deserialize(stream);
                    return statuses;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка десериализации лога");
                return null;
            }

        }
    }
}
