using System;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("5623376A-256B-41E3-BA35-0A3D1833035F"), ComVisible(true)]
    public interface ICache
    {
        /// <summary>
        /// 
        /// </summary>
        [DispId(1)] int PumpMessagesInterval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        [DispId(2)] void Remove(string Key);

        /// <summary>
        /// 
        /// </summary>
        [DispId(3)] void ClearCache();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime"></param>
        [DispId(4)] void Set(string Key, object Value, double CacheTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="CallMaximumCallFrequency"></param>
        /// <returns></returns>
        [DispId(5)] object Get(string Key, double CallMaximumCallFrequency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime"></param>
        [DispId(6)] void SetDouble(string Key, double Value, double CacheTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="CallMaximumCallFrequency"></param>
        /// <returns></returns>
        [DispId(7)] double GetDouble(string Key, double CallMaximumCallFrequency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime"></param>
        [DispId(8)] void SetInt(string Key, int Value, double CacheTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="CallMaximumCallFrequency"></param>
        /// <returns></returns>
        [DispId(9)] int GetInt(string Key, double CallMaximumCallFrequency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime"></param>
        [DispId(10)] void SetString(string Key, string Value, double CacheTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="CallMaximumCallFrequency"></param>
        /// <returns></returns>
        [DispId(11)] string GetString(string Key, double CallMaximumCallFrequency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime"></param>
        [DispId(12)] void SetBool(string Key, bool Value, double CacheTime);

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
/// <param name="CallMaximumCallFrequency"></param>
/// <returns></returns>
        [DispId(13)] bool GetBool(string Key, double CallMaximumCallFrequency);

    }

    /// <summary>
    /// 
    /// </summary>
    [ComVisible(false)]
    public interface ICacheExtra
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        object Get(string Key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        double GetDouble(string Key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        int GetInt(string Key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        string GetString(string Key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        bool GetBool(string Key);
    }
}
