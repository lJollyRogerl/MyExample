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
    public class SessionStatusesArray
    {
        public List<SessionStatuses> StatusesList { get; set; } = new List<SessionStatuses>();
        public SessionStatusesArray()
        {
        }

        public SessionStatusesArray(MM_MK_Collection currentDisplayedCol)
        {
            try
            {
                SessionStatusesArray array = StateSerialiser.DeSerializeList();
                if (array == null)
                {
                    StatusesList = new List<SessionStatuses>();
                    SessionStatuses status = new SessionStatuses();
                    status.MakeStates(currentDisplayedCol);
                    StatusesList.Add(status);
                    StateSerialiser.Serialize(this);
                    MessageBox.Show("Данных нет!", "Загрузка лога");
                }
                else
                {
                    StatusesList = array.StatusesList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "В конструкторе");
            }
        }
    }


    [Serializable]
    public class SessionStatuses
    {
        public List<PreviousSessionState> Statuses { get; set; } = new List<PreviousSessionState>();
        public SessionStatuses()
        {
            //try
            //{
            //    Statuses = StateSerialiser.DeSerialize().Statuses;
            //    if (Statuses == null)
            //    {
            //        Statuses = new List<PreviousSessionState>();
            //        MessageBox.Show("Данных нет!", "Загрузка лога");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "В конструкторе");
            //}
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
        public static void Serialize(SessionStatusesArray statusesList)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Append,
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

        public static SessionStatusesArray DeSerializeList()
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                SessionStatusesArray statuses;
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Open,
                    FileAccess.Read, FileShare.None))
                {

                    statuses = (SessionStatusesArray)binary.Deserialize(stream);
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
