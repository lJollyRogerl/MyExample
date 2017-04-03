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
        private static bool isLoaded = false;
        public List<PreviousSessionStatuses> Sessions { get; set; }
        public SessionsArray()
        {
        }
        //Если нет сериализованного лога, создаем лог и записываем текущие значения.
        //Если есть - достаем лог оттуда
        public SessionsArray(MM_MK_Collection currentDisplayedCol)
        {
            try
            {
                SessionsArray array = StateSerialiser.DeSerializeList();
                if (array == null)
                {
                    Sessions = new List<PreviousSessionStatuses>();
                    PreviousSessionStatuses status = new PreviousSessionStatuses();
                    status.MakeStates(currentDisplayedCol);
                    Sessions.Add(status);
                    DoSerialization();
                }
                else
                {
                    Sessions = array.Sessions;
                    //Если лог уже есть, но в данной сессии загрузка происходит первый раз - добавить текущие значения в лог
                    if (isLoaded == false)
                    {
                        Add(currentDisplayedCol);
                        isLoaded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка создания лога");
            }
        }

        public void DoSerialization()
        {
            StateSerialiser.Serialize(this);
        }

        //Добавляет к логу текущую сессию а затем серриализует
        public void Add(MM_MK_Collection currentDisplayedCol)
        {
            try
            {
                PreviousSessionStatuses status = new PreviousSessionStatuses();
                status.MakeStates(currentDisplayedCol);
                Sessions.Add(status);
                DoSerialization();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления лога");
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
                foreach (var unit in currentCollection.TheCollection)
                {
                    PreviousSessionState state = new PreviousSessionState();
                    state.TheDate = DateTime.Now;
                    string status = "";
                    if (unit.IsOnline)
                        status = "Был в сети.";
                    else
                    {
                        status = "Не был в сети.";
                        if (!string.IsNullOrWhiteSpace(unit.LastDateOnline))
                            status += $" Последняя сессия {unit.LastDateOnline}";
                    }
                    state.TitleAndState = unit.Title + " " + unit.MainOrReserve + " " + status;
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
