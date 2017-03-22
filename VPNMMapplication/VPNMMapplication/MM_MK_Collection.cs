using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNMMapplication
{
    public class MM_MK_Collection
    {
        public List<MM_MK_Unit> TheCollection { get; set; } = new List<MM_MK_Unit>();

        public void Add(MM_MK_Unit unit)
        {
            TheCollection.Add(unit);
        }

        public void AddCollection(MM_MK_Collection collectionToAdd)
        {
            TheCollection.AddRange(collectionToAdd.TheCollection);
        }

        public MM_MK_Unit this[string name]
        {
            get
            {
                foreach (MM_MK_Unit unit in TheCollection)
                {
                    if (unit.Title == name)
                        return unit;
                }
                return null;
            }

            //Inserts a units info if name is the same 
            private set
            {
                foreach (MM_MK_Unit unit in TheCollection)
                {
                    if (unit.Title == name)
                    {
                        unit.DNS_Name = value.DNS_Name;
                        unit.IP = value.IP;
                        unit.IsOnline = value.IsOnline;
                    }
                }
            }

        }
    }

    public class MM_MK_Unit
    {
        public string Title { get; set; }
        public string DNS_Name { get; set; }
        public string IP { get; set; }
        public bool IsOnline { get; set; }
        public string MainOrReserve { get; set; }
        public string LastDateOnline { get; set; }
    }
}
