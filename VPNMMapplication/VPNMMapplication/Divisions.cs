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


    //Region which contain List of Filial names
    [Serializable]
    public class Region
    {
        public Region()
        {
            Filials = new List<string>();
        }
        public Region(string name)
        {
            Filials = new List<string>();
            NameOfRegion = name;
        }

        public string NameOfRegion { get; set; }
        public List<String> Filials { get; set; }

        //Индексатор для поиска филиала
        public string this[string name]
        {
            get
            {
                foreach (string fil in Filials)
                {
                    if (fil == name)
                        return fil;
                }
                return null;
            }
        }
    }

    //class Filial
    //{
    //    public Filial(string name, Region parent)
    //    {
    //        Name = name;
    //        Parent = parent;
    //    }
    //    public string Name { get; set; }
    //    public Region Parent { get; set; }
    //}
}
