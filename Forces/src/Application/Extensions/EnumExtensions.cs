using System;
using System.ComponentModel;
using System.Linq;

namespace Forces.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? attributes[0].Description
                : val.ToString();
        }
        public static string ToArDescriptionString(this Enum val)
        {
           // return val.GetAttribute<ArDescription>().Name;
            var attributes = (ArDescription[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(ArDescription), false);

            return attributes.Length > 0
                ? attributes[0].Name
                : val.ToString();
        }
        public static string ToImagePath(this Enum val)
        {
            var attributes = (imagePath[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(imagePath), false);

            return attributes.Length > 0
                ? attributes[0].img
                : "00";
        }
        public static string ToLocalizedDescriptionString(this Enum val,string LangCode) 
        {
            if (LangCode == "ar-AR") 
            {
                return ToArDescriptionString(val);
            }
            else if (LangCode == "en-US")
            {
                return ToEnDescriptionString(val);
            }
            return ToDescriptionString(val);
        }
        public static string ToEnDescriptionString(this Enum val)
        {
            //return val.GetAttribute<EnDescription>().Name;
            var attributes = (EnDescription[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(EnDescription), false);

            return attributes.Length > 0
                ? attributes[0].Name
                : val.ToString();
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

    }
    public class ArDescription : Attribute
    {
        public string Name { get; private set; }

        public ArDescription(string ArabicDiscription)
        {
            this.Name = ArabicDiscription;
        }
    }
    public class EnDescription : Attribute
    {
        public string Name { get; private set; }

        public EnDescription(string EnglishDiscription)
        {
            this.Name = EnglishDiscription;
        }
    }
    public class imagePath : Attribute
    {
        public string img { get; private set; }
        public imagePath(string image)
        {
                this.img = image;
        }

    }

}