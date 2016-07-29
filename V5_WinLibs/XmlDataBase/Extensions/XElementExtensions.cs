using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Collections;

namespace XmlDatabase
{
    static class XElementExtensions
    {
        public static Entity ConvertToObject<T>(this XElement e) {
            //将元素的Attribute解释为对象的属性，这些对象属性必须可读可写

            Guid id = default(Guid);

            Entity entity = new Entity()
                {
                    Instance = GetTypeInstance(e, typeof(T), true, out id),
                    Id = id
                };

            return entity;
        }

        public static XElement ConvertToXml(this object instance,string elementName,Guid uuid) {
            return GetTypeElement(instance.GetType(), instance, elementName, uuid);
        }

        public static XElement ConvertToXml(this object instance,Guid uuid) {
            return ConvertToXml(instance, instance.GetType().Name,uuid);
        }

        public struct Entity
        {
            public Guid Id { get; set; }
            public object Instance { get; set; }

        }

        private static object GetTypeInstance(XElement e, Type t,bool topLevel,out Guid id)
        {
            //每个元素都检查一个TypeName属性，这表示是一个特定类型，如此的话，则需要进入递归
            string typeName = t.FullName;
            object instance = t.Assembly.CreateInstance(typeName);

            var writableProperties = t.GetProperties().Where(_p => _p.GetSetMethod() != null);

            foreach (var p in writableProperties)
            {
                Type pType = p.PropertyType;
                string fullType = pType.FullName;
                //这里仍然是分几种情况
                if (!generalTypes.Contains(fullType))
                {
                    if (pType.IsEnum)//如果是枚举
                    {
                        XElement item = e.Element(p.Name);
                        if (item != null)
                        {
                            p.SetValue(instance, Enum.Parse(p.PropertyType, item.Attribute("Enum").Value), null);

                        }
                    }
                    else if (pType.GetInterface(typeof(IList).FullName) != null && pType.IsGenericType)//如果是泛型集合
                    {

                        IList list = (IList)typeof(List<string>).Assembly.CreateInstance(pType.FullName);
                        Type type = pType.GetGenericArguments()[0];

                        XElement items = e.Element(p.Name);
                        foreach (var item in items.Elements())
                        {
                            list.Add(GetTypeInstance(item, type,false,out id));
                        }
                        p.SetValue(instance, list, null);

                    }
                    else if (pType.IsClass || pType.IsValueType)
                    {
                        XElement item = e.Element(p.Name);//通过一个元素找到该属性
                        if (item != null)
                        {

                            p.SetValue(instance, GetTypeInstance(item, pType,false,out id), null);

                        }
                    }
                }
                else
                {
                    //这种情况最简单，直接读取XAttbite作为属性
                    p.SetValue(instance, Convert.ChangeType(e.Attribute(p.Name).Value,pType), null);
                }

            }


            if (topLevel && e.Attribute("_uuid") != null)
            {
                id = new Guid(e.Attribute("_uuid").Value);
            }
            else
                id = Guid.Empty;

            return instance;

        }

        private static XElement GetTypeElement(Type t,object instance, string name,Guid uuid)
        {
            //元素的设计要点：
            //1.所有的行都是用Row为元素的，该元素有一个属性(Attribute)为类型完整名称
            //2.该类型的所有的标准属性（使用系统内置类型的），全部作为Row元素的Attribute
            //3.复杂类型的话，要另起一个子元素，如下面的Customer
            //4.如何是集合的话，则要另起一个复杂的子元素，如下面的OrderItems
            XElement temp = new XElement(name);

            if (uuid != Guid.Empty)
                temp.Add(new XAttribute("_uuid", uuid.ToString()));//如果是顶级元素，则设置一个uuid

            var readableProperties = t.GetProperties().Where(_p => _p.GetGetMethod() != null);

            foreach (var p in readableProperties)//循环所有属性(可读属性）
            {
                Type pType = p.PropertyType;
                string fullType = pType.FullName;

                //这里仍然是分几种情况
                if (!generalTypes.Contains(fullType))
                {
                    if (pType.IsEnum)//如果是枚举
                    {
                        XElement item = new XElement(p.Name);
                        item.Add(
                            new XAttribute("Enum", p.GetValue(instance, null)));

                        temp.Add(item);
                    }
                    else if (pType.GetInterface(typeof(IList).FullName) != null && pType.IsGenericType)//如果是泛型集合
                    {
                        IList list = (IList)p.GetValue(instance, null);
                        if (list != null && list.Count > 0)
                        {
                            XElement items = new XElement(p.Name);
                            foreach (var item in list)
                            {

                                XElement e1 =
                                    GetTypeElement(item.GetType(), item, p.Name.Substring(0, p.Name.Length - 1),Guid.Empty);//进入递归
                                items.Add(e1);
                            }

                            temp.Add(items);
                        }

                    }
                    else if (pType.IsClass || pType.IsValueType)
                    {
                        object o = p.GetValue(instance, null);
                        temp.Add(GetTypeElement(pType, o, p.Name,Guid.Empty));//进入递归
                    }
                }
                else
                {//这种情况最简单，属性为标准内置类型，直接作为元素的Attribute即可
                    temp.Add(
                        new XAttribute(p.Name, p.GetValue(instance, null)));

                }
            }
            return temp;
        }


        private static readonly List<string> generalTypes = new List<string>()
        {
            "System.Byte",//typeof(byte).FullName,
            "System.SByte",//typeof(sbyte).FullName,
            "System.Int16",//typeof(short).FullName,
            "System.UInt16",//typeof(ushort).FullName,
            "System.Int32",//typeof(int).FullName,
            "System.UInt32",//typeof(uint).FullName,
            "System.Int64",//typeof(long).FullName,
            "System.UInt64",//typeof(ulong).FullName,
            "System.Double",//typeof(double).FullName,
            "System.Decimal",//typeof(decimal).FullName,
            "System.Single",//typeof(float).FullName,
            "System.Boolean",//typeof(bool).FullName,
            "System.String",//typeof(string).FullName,
            "System.DateTime"//typeof(DateTime).FullName
        };

    }
}
