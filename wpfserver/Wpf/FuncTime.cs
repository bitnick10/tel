using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf
{
     internal class FuncTimeData
    {
        UInt64 id;
        string name;
        UInt64 time;
        public UInt64 ID{get{return id;}set{id=value;}}
        public string Name{get{return name;}set{name=value;}}
        public UInt64 Time{get{return time;}set{time=value;}}
        public FuncTimeData(UInt64 id, string name, UInt64 time)
        {
            this.id = id;
            this.name = name;
            this.time = time;
        }
    }
    class FuncTime
    {
        List<FuncTimeData> data=new List<FuncTimeData>();
        public void Add(UInt64 id,string name,UInt64 time)
        {
            data.Add(new FuncTimeData(id, name, time));
        }
    }
}
