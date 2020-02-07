﻿
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common
{

public  class Enums
{
        public enum NotifyType
        {

            [Display(Name = "Success")]
            [Description("Success")]
            Success = 1,


            [Display(Name = "Error")]
            [Description("Error")]
            Error = 0
        }

        #region Property

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

   /// <summary>
   /// Gets or sets the name.
   /// </summary>
   /// <value>The name.</value>
   public string Name { get; set; }

   #endregion

   #region Methods

   /// <summary>
   /// Get the ENUM Name from ENUM Value
   /// </summary>
   /// <param name="objEnumType">ENUM Type like type of(ENUM Type)</param>
   /// <param name="value">ENUM value</param>
   /// <returns>string - Name of ENUM</returns>
   public static string GetEnumName(Type objEnumType, int value)
   {
       EnumList lstEnum = GetEnumList(objEnumType);
       Enums objSystemEnum;
       objSystemEnum = lstEnum.Find(delegate(Enums systemEnum)
       {
           return systemEnum.ID == value;
       });

       if (objSystemEnum != null)
       {
           return objSystemEnum.Name;
       }
       else
       {
           return string.Empty;
       }
   }

   /// <summary>
   /// Get the ENUM List from given ENUM type
   /// </summary>
   /// <param name="objEnumType">ENUM Type like type-of(ENUM Type)</param>
   /// <returns>List of ENUM with Name Value pair</returns>
   public static EnumList GetEnumList(Type objEnumType)
   {
       Array values = Enum.GetValues(objEnumType);
       EnumList lstEnum = new EnumList();
       Enums objEnum;
       for (int i = 0; i < values.Length; i++)
       {
           objEnum = new Enums();
           objEnum.ID = values.GetValue(i).GetHashCode();
           objEnum.Name = Convert.ToString(values.GetValue(i)).Replace("_", " ");
           lstEnum.Add(objEnum);
       }

       return lstEnum;
   }

   /// <summary>
   /// Get the ENUM List from given ENUM type
   /// To Add Custom option in list
   /// </summary>
   /// <param name="objEnumType">ENUM Type like type-of(ENUM Type)</param>
   /// <param name="optional">The optional.</param>
   /// <returns>List of ENUM with Name Value pair</returns>
   public static EnumList GetEnumList(Type objEnumType, string optional)
   {
       Array values = Enum.GetValues(objEnumType);
       EnumList lstEnum = new EnumList();
       Enums objEnum;
       if (!string.IsNullOrEmpty(optional))
       {
           lstEnum.Add(new Enums() { Name = optional });
       }

       for (int i = 0; i < values.Length; i++)
       {
           objEnum = new Enums();
           objEnum.ID = values.GetValue(i).GetHashCode();
           objEnum.Name = Convert.ToString(values.GetValue(i)).Replace("_", " ");
           lstEnum.Add(objEnum);
       }

       return lstEnum;
   }

   
   #endregion
}

  /// <summary>
    /// Class ENUMS List.
    /// </summary>
    public class EnumList : List<Enums>
    {
    }

	public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum enumValue)
        {
            object[] attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length > 0
               ? ((DescriptionAttribute)attr[0]).Description
               : enumValue.ToString();
        }

        public static T ParseEnum<T>(this string stringVal)
        {
            return (T)Enum.Parse(typeof(T), stringVal);
        }
    }
}
