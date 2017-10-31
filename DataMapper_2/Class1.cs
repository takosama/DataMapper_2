using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper_2
{
    class Mapper
    {
        public static T MapDynamic<T>(dynamic obj) where T : new()
        {
            var rtn = new T();

            foreach (var n in obj)
            {
                var type = n.Key;
                var val = n.Value;
                try
                {
                    rtn.GetType().GetProperty(type).SetValue(rtn, val);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return rtn;
        }

        static public T MapDynamicLiteralObjectEntity<T>(JsDataParser.Entities.IDynamicLiteralObjectEntity obj) where T : new()
        {
            var rtn = new T();
            foreach (var n in rtn.GetType().GetProperties())
            {
                string name = n.Name;
                if (!obj.TryGetField(name, out dynamic tmp)) continue;

                var val = (JsDataParser.Entities.ValueEntity)tmp;

                object setObj;
                if (val.ValueType == JsDataParser.Entities.ValueTypes.Array)
                {
                    setObj = val.Array.Select(x => x.Object).ToArray();
                }
                else
                    setObj = val.Object;
                rtn.GetType().GetProperty(name).SetValue(rtn, setObj);
            }
            return rtn;
        }


    }


}
