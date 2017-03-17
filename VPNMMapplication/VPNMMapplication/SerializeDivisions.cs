using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace VPNMMapplication
{
    //Serializator for regions anf filials
    public class SerializeDivisions
    {
        public static List<Region> GetListOfRegions()
        {
            try
            {
                List<Region> regions = new List<Region>();
                XmlSerializer serializer = new XmlSerializer(typeof(Divisions), new Type[] { typeof(Region), typeof(Filial) });
                using (FileStream fs = new FileStream("ListOfFillials.xml", FileMode.Open))
                {
                    Divisions div = (Divisions)serializer.Deserialize(fs);
                    regions = div.Regions;
                }
                return regions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка десериализации");
                return null;
            }

        }

        public static void AddFillial(Divisions currentDivision, Filial newFilial)
        {
            //Если такой регион уже существует, тогда добавляем в него филиал.
            //Если такой филиал уже есть в данном регионе - сообщаем пользователю об этом
            if (currentDivision[newFilial.ParentRegion.NameOfRegion] != null)
            {
                if (currentDivision[newFilial.ParentRegion.NameOfRegion][newFilial.Name] == null)
                    currentDivision[newFilial.ParentRegion.NameOfRegion].Filials.Add(newFilial);
                else
                    MessageBox.Show("Филиал с таким названием уже существует!", "Ошибка!");
            }
            //Если такого региона еще не существует - создаем новый и туда добавляем филиал
            else
            {
                currentDivision.Regions.Add(new Region(newFilial.ParentRegion.NameOfRegion));
                currentDivision[newFilial.ParentRegion.NameOfRegion].Filials.Add(newFilial);
            }
            //нарезаем в хмл
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Divisions), new Type[] { typeof(Region), typeof(Filial) });
                using (FileStream fs = new FileStream("ListOfFillials.xml", FileMode.OpenOrCreate))
                {
                    serializer.Serialize(fs, currentDivision);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сериализации!");
            }
        }
    }
}
