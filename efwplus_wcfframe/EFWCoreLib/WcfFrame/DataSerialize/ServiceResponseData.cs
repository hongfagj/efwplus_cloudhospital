﻿using EFWCoreLib.WcfFrame.SDMessageHeader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using ProtoBuf.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.DataSerialize
{
    /// <summary>
    /// 服务输出数据
    /// </summary>
    public class ServiceResponseData
    {
        //string _retData;
        List<string> _listjson;
        //List<Object> listdata;
        public ServiceResponseData()
        {
            //listdata = new List<object>();
            _listjson = new List<string>();
        }

        public ServiceResponseData(bool IsCompress, bool IsEncrytion, SerializeType SerializeType)
        {
            //listdata = new List<object>();
            _iscompressjson = IsCompress;
            _isencryptionjson = IsEncrytion;
            _serializetype = SerializeType;
            _listjson = new List<string>();
        }
        bool _iscompressjson = false;
        bool _isencryptionjson = false;
        SerializeType _serializetype = SerializeType.Newtonsoft;

        public bool Iscompressjson
        {
            get
            {
                return _iscompressjson;
            }

            set
            {
                _iscompressjson = value;
            }
        }

        public bool Isencryptionjson
        {
            get
            {
                return _isencryptionjson;
            }

            set
            {
                _isencryptionjson = value;
            }
        }

        public SerializeType Serializetype
        {
            get
            {
                return _serializetype;
            }

            set
            {
                _serializetype = value;
            }
        }

        public void AddData<T>(T data)
        {
            if (_serializetype == SerializeType.Newtonsoft)
            {
                if (data is DataTable)
                {
                    _listjson.Add(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                else
                {
                    _listjson.Add(JsonConvert.SerializeObject(data));
                }
            }
            else if (_serializetype == SerializeType.protobuf)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (data is DataTable)
                    {
                        object obj = data;
                        DataSerializer.Serialize(ms, (DataTable)obj);
                    }
                    else
                    {
                        Serializer.Serialize(ms, data);
                    }
                    _listjson.Add(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                }
            }
        }

        public string GetJsonData()
        {
            return JsonConvert.SerializeObject(_listjson);
        }

        public void SetJsonData(string retData)
        {
            //_retData = retData;
            _listjson = JsonConvert.DeserializeObject<List<string>>(retData);
        }

        public T GetData<T>(int index)
        {
            if (_serializetype == SerializeType.Newtonsoft)
            {
                return JsonConvert.DeserializeObject<T>(_listjson[index]);
            }
            else if (_serializetype == SerializeType.protobuf)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(_listjson[index]);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    if (default(T) is DataTable)
                    {
                        Object obj = DataSerializer.DeserializeDataTable(ms);
                        return (T)obj;
                    }
                    else
                    {
                        return Serializer.Deserialize<T>(ms);
                    }
                }
            }
            else
                return default(T);
        }

        /*

        #region  值转换
        private object convertVal(Type t, object _data)
        {
            string data = _data.ToString();
            object val = null;
            if (t == typeof(Int32))
                val = Convert.ToInt32(data);
            else if (t == typeof(DateTime))
                val = Convert.ToDateTime(data);
            else if (t == typeof(Decimal))
                val = Convert.ToDecimal(data);
            else if (t == typeof(Boolean))
                val = Convert.ToBoolean(data);
            else if (t == typeof(String))
                val = Convert.ToString(data).Trim();
            else if (t == typeof(Guid))
                val = new Guid(data.ToString());
            else if (t == typeof(byte[]))
                if (data != null && data.ToString().Length > 0)
                {
                    val = Convert.FromBase64String(data.ToString());
                }
                else
                {
                    val = null;
                }
            else
                val = data;
            return val;
        }
        #endregion

        #region IToJson 成员
        public string ToJson(object model)
        {
            string value = JsonConvert.SerializeObject(model);
            return value;
        }
        public string ToJson(System.Data.DataTable dt)
        {
            string value = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return value;
        }
        public string ToJson(string data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(int data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(decimal data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(bool data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(DateTime data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(params object[] data)
        {
            string value = JsonConvert.SerializeObject(data);
            return value;
        }

        #endregion

        #region IJsonToObject成员
        public T ToObject<T>(string json)
        {
            T t = NewObject<T>();
            ConvertExtend.ToObject(JsonConvert.DeserializeObject<T>(json), t);
            return t;
        }
        public object ToObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
        public object[] ToArray(string json)
        {
            return (ToObject(json) as JArray).ToArray();
        }

        public T[] ToArray<T>(string json)
        {
            return Array.ConvertAll<object, T>(ToArray(json), o =>
            {
                if (typeof(T).Name == typeof(int).Name)
                {
                    return (T)((object)Convert.ToInt32(o.ToString()));
                }
                else if (typeof(T).Name == typeof(string).Name)
                {
                    return (T)((object)(o.ToString()));
                }
                return default(T);
            });
        }

        public string ToString(string json)
        {
            return Convert.ToString(ToArray(json)[0].ToString());
        }
        public bool ToBoolean(string json)
        {
            return Convert.ToBoolean(ToArray(json)[0].ToString());
        }
        public int ToInt32(string json)
        {
            return Convert.ToInt32(ToArray(json)[0].ToString());
        }
        public decimal ToDecimal(string json)
        {
            return Convert.ToDecimal(ToArray(json)[0].ToString());
        }
        public DateTime ToDateTime(string json)
        {
            return Convert.ToDateTime(ToArray(json)[0].ToString());
        }

        public T ToObject<T>(object data)
        {
            if (typeof(T).Equals(typeof(int[])))
            {
                List<int> intvals = new List<int>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add(Convert.ToInt32((data as JArray)[i].ToString()));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(string[])))
            {
                List<string> intvals = new List<string>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add(Convert.ToString((data as JArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(decimal[])))
            {
                List<decimal> intvals = new List<decimal>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add(Convert.ToDecimal((data as JArray)[i].ToString()));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(Boolean[])))
            {
                List<Boolean> intvals = new List<Boolean>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add(Convert.ToBoolean((data as JArray)[i].ToString()));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(DateTime[])))
            {
                List<DateTime> intvals = new List<DateTime>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add(Convert.ToDateTime((data as JArray)[i].ToString()));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(object[])))
            {
                List<object> intvals = new List<object>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    intvals.Add((data as JArray)[i].ToString());
                }
                return (T)(intvals.ToArray() as object);
            }
            else
            {
                T obj = NewObject<T>();
                PropertyInfo[] pros = typeof(T).GetProperties();
                for (int k = 0; k < pros.Length; k++)
                {
                    object val = convertVal(pros[k].PropertyType, (data as JObject)[pros[k].Name]);
                    pros[k].SetValue(obj, val, null);
                }
                return obj;
            }
        }
        public object[] ToArray(object data)
        {
            return (data as JArray).ToArray();
        }
        public T[] ToArray<T>(object data)
        {
            return Array.ConvertAll<object, T>(ToArray(data), o =>
            {
                if (typeof(T).Name == typeof(int).Name)
                {
                    return (T)((object)Convert.ToInt32(o.ToString()));
                }
                else if (typeof(T).Name == typeof(string).Name)
                {
                    return (T)((object)(o.ToString()));
                }
                return default(T);
            });
        }
        public List<T> ToListObj<T>(object data)
        {
            if (data is JArray)
            {
                PropertyInfo[] pros = typeof(T).GetProperties();
                List<T> list = new List<T>();
                for (int i = 0; i < (data as JArray).Count; i++)
                {
                    T obj = NewObject<T>();
                    object _data = (data as JArray)[i];
                    for (int k = 0; k < pros.Length; k++)
                    {
                        object val = convertVal(pros[k].PropertyType, (_data as JObject)[pros[k].Name]);
                        pros[k].SetValue(obj, val, null);
                    }
                    list.Add(obj);
                }
                return list;
            }

            return null;
        }

        public DataTable ToDataTable(Object data)
        {
            return ToDataTable(data.ToString());
        }

        public DataTable ToDataTable(string data)
        {
            return JsonConvert.DeserializeObject<DataTable>(data);
        }
        #endregion
        */
    }
}