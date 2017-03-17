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
        }
        public void DivisionLoad()
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
                foreach (Region reg in Regions)
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

    //Region which contain List of Filials
    [Serializable]
    public class Region
    {
        public List<Filial> Filials { get; set; }
        public string NameOfRegion { get; set; }

        public Region()
        {
            Filials = new List<Filial>();
        }
        public Region(string name)
        {
            Filials = new List<Filial>();
            NameOfRegion = name;
        }

        //Индексатор для поиска филиала
        public Filial this[string name]
        {
            get
            {
                for (int i = 0; i < Filials.Count; i++)
                {
                    if (Filials[i].Name == name)
                        return Filials[i];
                }
                return null;
            }
        }
    }

    [Serializable]
    public class Filial
    {
        private Filial()
        {
        }
        public Filial(string name, Region parent)
        {
            Name = name;
            ParentRegion = parent;
        }
        public string Name { get; set; }
        [XmlElement(ElementName = "Parent_Region")]
        public Region ParentRegion { get; set; }
    }
}