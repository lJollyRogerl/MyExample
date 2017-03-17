using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace VPNMMapplication
{
    //Рутовский тег в xml. Содержит список регионов, которые в свою очередь  - списки филиалов
    [Serializable]
    public class Divisions
    {
        public Divisions()
        {
            //Если в сборке присутствует список филлиалов - загрузить его. Если нет - создать новый
            List<Region> regions = SerializeDivisions.GetListOfRegions();
            if (regions != null)
                Regions = regions;
            else
                Regions = new List<Region>();
        }

        public List<Region> Regions { get; set; }
        public Region this[string name]
        {
            get
            {
                foreach(Region reg in Regions)
                {
                    if (reg.NameOfRegion == name)
                        return reg;
                }
                return null;
            }

            //Inserts a list of fillials in choosen Region
            private set
            {
                foreach (Region reg in Regions)
                {
                    if (reg.NameOfRegion == name)
                        reg.Filials = value.Filials;
                }
            }
        }
    }

    [Serializable]
    public class Region
    {
        public Region()
        {
        }
        public Region(string name)
        {
            NameOfRegion = name;
        }
        public string NameOfRegion { get; set; }
        public List<String> Filials { get; set; } = new List<String>();
    }

    public class SerializeDivisions
    {
        public static List<Region> GetListOfRegions()
        {
            try
            {
                List<Region> regions = new List<Region>();
                XmlSerializer serializer = new XmlSerializer(typeof(Divisions));
                using (FileStream fs = new FileStream("ListOfFillials.xml", FileMode.Open))
                {
                    Divisions div = (Divisions)serializer.Deserialize(fs);
                    regions = div.Regions;
                }
                return regions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка десериализации!");
                return null;
            }
            
        }

        public static void AddFillial(Divisions currentDivision, string newFilial, string region)
        {
            if (currentDivision[region] != null)
            {
                currentDivision[region].Filials.Add(newFilial);
            }
            else
            {
                currentDivision.Regions.Add(new Region(region));
                currentDivision[region].Filials.Add(newFilial);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Divisions));
                using (FileStream fs = new FileStream("ListOfFillials.xml", FileMode.Open))
                {
                    Divisions div = (Divisions)serializer.Deserialize(fs);
                    regions = div.Regions;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка серриализации!");
            }
        }
    }
}
