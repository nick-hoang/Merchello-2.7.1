﻿// <auto-generated/> - StyleCop hack to not enforce commentation errors in this file.
namespace Merchello.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Merchello.Core.Logging;
    using Merchello.Core.Models;
    using Merchello.Core.Models.Interfaces;

    /// <summary>
    /// Extension methods for <see cref="IDataModifierData"/>.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Modifies property data.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        /// <typeparam name="T">
        /// The type of <see cref="IDataModifierData"/>
        /// </typeparam>
        public static void ModifyData<T>(this T value, string propertyName, object newValue)
            where T : class, IDataModifierData
        {
            value.ModifyData(propertyName, newValue, new ExtendedDataCollection());
        }

        /// <summary>
        /// Modifies property data.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        /// <param name="extendedData">
        /// The <see cref="ExtendedDataCollection"/>
        /// </param>
        /// <typeparam name="T">
        /// The type of <see cref="IDataModifierData"/>
        /// </typeparam>
        public static void ModifyData<T>(this T value, string propertyName, object newValue, ExtendedDataCollection extendedData)
            where T : class, IDataModifierData
        {
            var propInfo = value.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propInfo == null || !propInfo.CanWrite || !propInfo.CanRead)
            {
                MultiLogHelper.Debug(typeof(Extensions), "Could not change property: " + propertyName);
                return;
            }

            try
            {
                var oldValue = propInfo.GetValue(value, null);
                propInfo.SetValue(value, newValue, null);
                var log = new DataModifierLog()
                {
                    PropertyName = propertyName,
                    OriginalValue = oldValue,
                    ModifiedValue = newValue,
                    ExtendedData = extendedData
                };

                var logs = value.ModifiedDataLogs == null ?
                    new List<IDataModifierLog>() :
                    value.ModifiedDataLogs as List<IDataModifierLog>;
                if (logs != null)
                {
                    logs.Add(log);
                    value.ModifiedDataLogs = logs;
                }
            }
            catch (Exception ex)
            {
                MultiLogHelper.Error(typeof(Extensions), "Failed to set property: " + propertyName, ex);
            }
        }
    }
}
