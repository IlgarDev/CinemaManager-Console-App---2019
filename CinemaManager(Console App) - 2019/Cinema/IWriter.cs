using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    interface IWriter
    {
        void SerializeToXml(string path);
        void DeSerializeToXml(string path);
    }
}
