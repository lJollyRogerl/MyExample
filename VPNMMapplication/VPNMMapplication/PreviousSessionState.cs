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
    public class SessionStatuses
    {
        public SessionStatuses()
        {
            Statuses = StateSerialiser.DeSerialize();
            if (Statuses == null)
            {
                Statuses = new List<PreviousSessionState>();
                MessageBox.Show("Данных нет!", "Загрузка лога");
            }
        }

        public void AddToLog(PreviousSessionState state)
        {
            StateSerialiser.AddStatus(state);
        }

        
        public List<PreviousSessionState> Statuses { get; set; }
    }

    [Serializable]
    public class PreviousSessionState
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

    public static class StateSerialiser
    {
        //Добавляет данные по поледней дате в бин файл
        public static void AddStatus(PreviousSessionState state)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Append,
                    FileAccess.Write, FileShare.None))
                {
                    binary.Serialize(stream, state);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сериализации лога");
            }
            
        }
        //Сериализует данные по поледним статусам в бин файл
        public static void Serialize(List<PreviousSessionState> statuses)
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Append,
                    FileAccess.Write, FileShare.None))
                {
                    binary.Serialize(stream, statuses);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сериализации лога");
            }

        }
        //Десериализует данные по поледним статусам из бин файла
        public static List<PreviousSessionState> DeSerialize()
        {
            try
            {
                BinaryFormatter binary = new BinaryFormatter();
                List<PreviousSessionState> statuses;
                using (Stream stream = new FileStream("LastStatuses.bin", FileMode.Open,
                    FileAccess.Read, FileShare.None))
                {

                    statuses = (List<PreviousSessionState>)binary.Deserialize(stream);
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
