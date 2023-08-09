using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.Utilities.Exceptions;

namespace ASCOM.Utilities
{
    /// <summary>
    /// ASCOM caching component with automatic stale item removal and call rate limiting - <i>this component must be disposed when the driver is closing down. See <see cref="Cache.Dispose()"/> for further information.</i>
    /// </summary>
    /// <remarks><para>Astronomy applications are becoming increasingly sophisticated and frequently employ multi-threading techniques that place increasingly high call rate burdens on drivers. 
    /// Much hardware remains of modest processing capacity and performance can suffer if hardware controllers have to process an excessive number of requests for information and one of the
    /// jobs of the driver is to protect the hardware from this kind of situation. Caching is a helpful technique to combat excessive polling of values that don't change frequently and this 
    /// component provides a conveniently packaged capability to ease driver development.</para>
    /// <para>This cache will store items against specified keys for configurable periods of time and, when an item exceeds its specified retention time, it will automatically be removed by the cache.</para>
    /// <para>Some applications poll at very high rates, so the cache provides an optional rate limiting capability that can delay execution of "set" and "get" methods to enforce a maximum number of actions per second.</para>
    /// <para>The cache "get" methods will either return the requested item, if present, or will throw a NotInCacheException exception, indicating that the driver should poll the hardware and store the value 
    /// in the cache before returning it to the caller.</para>
    /// <para>Clients using the cache through COM e.g. from scripting languages , Delphi etc. will find that for each group of overloaded methods e.g. GetDouble, only the method with the largest number of parameters
    /// is available. This is due to a COM limitation that doesn't allow access to method overloads. .NET clients have access to all overloads.</para>
    /// <para>Code example</para>
    /// <code>
    /// using ASCOM.Utilities;
    /// using ASCOM.Utilities.Exceptions;
    /// 
    /// Class Driver
    ///     const string RIGHT_ASCENSION = "Right Ascension";
    ///     Cache myCache = new Cache();
    ///     ...
    ///     double RightAscension
    ///     {
    ///         get
    ///         {
    ///             try
    ///             {
    ///                 return cache.GetDouble(RIGHT_ASCENSION); // Get the RA value from the cache, if present, without limiting the number of reads per second
    ///                 // or return cache.GetDouble(RIGHT_ASCENSION, 5.0); // Get the RA value from the cache, if present, limiting the number of reads of this value to 5 per second
    ///             }
    ///             catch (NotInCacheException) // Exception thrown because requested value is not in the cache - so get it from hardware, save to the cache and return the value
    ///             {
    ///                 double newRA = ... // Get value from hardware
    ///                 cache.SetDouble(RIGHT_ASCENSION, newRA, 1.0); // Save the new value to the cache with a timeout of 1 second
    ///                 return newRA;
    ///             }
    ///         }
    ///     }
    /// </code>
    /// </remarks>
    [Guid("01057AB6-76D6-4161-8D5E-992E6E1CD97C")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Cache : ICache, ICacheExtra, IDisposable
    {
        private const string MEMORY_CACHE_NAME = "ASCOM";
        private const int PUMP_MESSAGES_INTERVAL_DEFAULT = 0;
        private const int TIMER_RESOLUTION_REQUIRED = 1; // Timer resolution when making waits to enforce throttling
        private const double ALLOWABLE_THROTTLING_RATE_MINIMUM = 2.0;
        private const double ALLOWABLE_THROTTLING_RATE_MAXIMUM = 1000.0;

        private MemoryCache memoryCache;
        private TraceLogger TL;

        private Dictionary<string, DateTime> lastRead;
        private Dictionary<string, DateTime> lastWritten;

        private CacheEntryRemovedCallback removedCallBack;

        private int pumpMessagesInterval;
        private bool disposedValue = false; // IDisposable support - to detect redundant Dispose calls

        private Mutex syncMutex;

        // Static fields to hold the singleton timer resolution state across all instances of the Cache
        private static bool timerResolutionHasBeenChecked = false;
        private static bool timerResolutionHasBeenChanged = false;
        private static object timerResolutionChangeLock = new object(); // Static lock object to ensure that timer resolution changes happen in a threadsafe manner and only happen once
        private static int cacheInstanceCount = 0; // Number of concurrent cache instances 
        private static int timerResolutionCurrent = 0; // Value of timer resolution actually in effect

        private enum CacheAction
        {
            CacheRead,
            CacheWrite
        }

        #region Initialisers and IDisposable

        /// <summary>
        /// Default initialiser that does not assign a TraceLogger unless the cache tracing flag is set.
        /// </summary>
        /// <remarks>COM (as opposed to .NET) applications must use this initialiser because parameterised initialisers can not be used by COM applications.
        /// <para>Cache tracing can be enabled through the "Trace Cache" option that is set through the Chooser or Diagnostics menus. This will cause the cache to create a separate 
        /// log in the same directory as the application's own log.</para></remarks>
        public Cache()
        {
            Interlocked.Increment(ref cacheInstanceCount); // Increment the cache instance counter so we can reset the timer resolution if required when the last cache instance is no longer required

            memoryCache = new MemoryCache(MEMORY_CACHE_NAME);
            lastRead = new Dictionary<string, DateTime>();
            lastWritten = new Dictionary<string, DateTime>();
            syncMutex = new Mutex(false);
            pumpMessagesInterval = PUMP_MESSAGES_INTERVAL_DEFAULT;

            if (RegistryCommonCode.GetBool(GlobalConstants.TRACE_CACHE, GlobalConstants.TRACE_CACHE_DEFAULT))
            {
                TL = new TraceLogger("", "Cache")
                {
                    Enabled = true
                };
                TL.LogMessage("Initialiser", "Trace logger created OK");
            }
        }

        /// <summary>
        /// Inititialiser that takes a reference to the calling application's trace logger. This ensures that trace output will
        /// appear in context with the applications's own log messages, making it easier to debug the application.
        /// </summary>
        /// <remarks>The initialiser will accept null (Nothing) as a valid reference, this will suppress all cache logging.
        /// <para>Due to COM limitations, this initialiser can only be used by .NET applications. COM applications should use the parameterless constructor and, if required,
        /// enable cache tracing through the "Trace Cache" option that can be set through the Chooser or Diagnostics menus. This will cause the cache to create a separate 
        /// log in the same directory as the application's own log.</para></remarks>
        /// <param name="CallersLogger">Reference to the call's trace logger instance.</param>
        public Cache(TraceLogger CallersLogger) : this()
        {
            if (TL != null) // Clean up any tracelogger added by the default initialiser because the user has supplied their own TraceLogger
            {
                TL.Enabled = false;
                TL.Dispose();
                TL = null;
            }

            TL = CallersLogger; // Save the user's TraceLogger reference for use within this component
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                if (disposing)
                {
                    if (memoryCache != null)
                    {
                        memoryCache.Dispose();
                        memoryCache = null;
                    }
                    if (lastRead != null) lastRead = null;
                    if (lastWritten != null) lastWritten = null;
                }

                // Reset the timer resoution if its been changed and we are the last instance running - this needs to run whether called by the Dispose() method or by the GC finaliser
                lock (timerResolutionChangeLock) // Ensure that this code is serialised across all instances
                {
                    try
                    {
                        if ((Interlocked.Decrement(ref cacheInstanceCount) == 0) & (timerResolutionHasBeenChanged)) // Decrement the instance count and, if this is the end of last cache instance, reset the timer resolution if it has been changed
                        {
                            TimeEndPeriod(timerResolutionCurrent); // Reset the timer resolution to its original value
                            timerResolutionHasBeenChanged = false; // Reset to default values so that the cache can be used again if required
                            timerResolutionHasBeenChecked = false;
                        }
                    }
                    catch { } // Just swallow any exceptions from this code
                }
            }
        }

        /// <summary>
        /// Reset the PC's timing resolution, if changed, and cleanly dispose of the cache's working memory.
        /// </summary>
        /// <remarks>
        /// <para>Windows PCs use regular interrupts to manage timers and form events; a common repeat period is 15.6 milliseconds, which corresponds to 64 interupts per second. Microsoft chose this as a general purpose compromise between 
        /// timer resolution and UI responsiveness, which are better served with higher interrupt frequencies and power consumption, which is improved with lower interrupt frequencies. Among other things, the interrupt frequency determnines the resolution of 
        /// the Windows "Sleep" method, which this cache uses to effect throttling.</para>
        /// <para>If a PC is using the standard 15.6ms interrupt period, sleep times will be rounded up to the nearest 15.6ms making a 1ms sleep last 15.6ms and a 16ms sleep last 31.2ms etc.
        /// To work round this, if required, the cache can increase the sleep timing resolution to the maximum supported by the PC or 1ms, whichever is the larger. The sleep resolution will only be changed when the 
        /// first GetXXX or SetXXX call is made that specifies a non-zero value for MaximumCallFrequency. If all calls specify a MaximumCallFrequency of 0.0, the cache will not change the timing resolution.</para>
        /// <para>The Cache.Dispose() method should be called when the driver is closing down in order to restore the original timing resolution. If this isn't done, Windows will still restore 
        /// the original value when the overall application terminates, but it remains good practice to dispose of objects at the proper time.</para>
        /// </remarks>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

        #region ICache implementation

        // Primary members accessible from both COM and .NET applications

        /// <summary>
        /// Get or set the interval, in milliseconds, between calls to Application.DoEvents to pump Windows messages when throttling the cache read rate.
        /// </summary>
        /// <remarks>Setting this property to zero will prevent any message pumping by the cache while waiting. The default value of the property is zero.</remarks>
        /// <exception cref="InvalidValueException">When PumpMessagesInterval is set less than 0.</exception>
        public int PumpMessagesInterval
        {
            get
            {
                return pumpMessagesInterval;
            }
            set
            {
                // Validate parameter
                if (value < 0) throw new InvalidValueException(string.Format("The ASCOM Cache pump messages interval must be zero or positive, supplied value is negative: {0}", value));

                pumpMessagesInterval = value;
                LogMessage("PumpMessagesInterval", string.Format("Messages set to be pumped every {0} milliseconds", pumpMessagesInterval));
            }
        }

        /// <summary>
        /// Remove all items from the cache.
        /// </summary>
        public void ClearCache()
        {
            Stopwatch sw = new Stopwatch();

            LogMessage("ClearCache", string.Format("Removing all {0} items from the cache", memoryCache.GetCount()));

            try
            {
                sw.Start();
                syncMutex.WaitOne(); // Protect removal and replacement of the cache with an application wide mutex
                MemoryCache oldCache = memoryCache; // Get a handle to the current cache
                memoryCache = new MemoryCache(MEMORY_CACHE_NAME); // Replace the current cache with a new empty cache
                oldCache.Dispose(); // Dispose of the old cache. This will call any registered callbacks so could be slow if the cache has 000's of entries
                sw.Stop();
            }
            finally
            {
                syncMutex.ReleaseMutex();
            }
            LogMessage("CacheClear", string.Format("ASCOM cache cleared in {0} milliseconds", sw.ElapsedMilliseconds));
        }

        /// <summary>
        /// Remove a an item with a specific name from the cache
        /// </summary>
        /// <param name="Key">Name of the item to remove.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <remarks>No exception is thrown if the specified item does not exist.</remarks>
        public void Remove(string Key)
        {
            object item;

            // Validate parameters
            if (string.IsNullOrEmpty(Key)) throw new InvalidValueException("The supplied ASCOM Cache key is null or empty when removing an item from the cache.");

            try
            {
                syncMutex.WaitOne();
                item = memoryCache.Remove(Key);
            }
            finally
            {
                syncMutex.ReleaseMutex();
            }

            if (item == null)
            {
                LogMessage("Remove", string.Format("Entry {0} not present in cache", Key));
            }
            else
            {
                LogMessage("Remove", string.Format("Entry {0} removed from cache", Key));
            }
        }

        /// <summary>
        /// Retrieve an object from the cache with the given name, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <returns>The requested object if present in the cache or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public object Get(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheRead, MaximumCallFrequency);
            return Get(Key); //Get the value
        }

        /// <summary>
        /// Retrieve a double value from the cache with the given name, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <returns>The requested double value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public double GetDouble(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheRead, MaximumCallFrequency);
            return GetDouble(Key); //Get the value
        }

        /// <summary>
        /// Retrieve an integer value from the cache with the given name, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <returns>The requested integer value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public int GetInt(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheRead, MaximumCallFrequency);
            return GetInt(Key); //Get the value
        }

        /// <summary>
        /// Retrieve a boolean value from the cache with the given name, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <returns>The requested boolean value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public bool GetBool(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheRead, MaximumCallFrequency);
            return GetBool(Key); //Get the value
        }

        /// <summary>
        /// Retrieve a string value from the cache with the given name, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <returns>The requested string value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public string GetString(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheRead, MaximumCallFrequency);
            return GetString(Key); //Get the value
        }

        /// <summary>
        /// Save an object in the cache with the given name and time to live, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Object to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <param name="MaximumCallFrequency">Maximum number of set calls per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void Set(string Key, object Value, double CacheTime, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheWrite, MaximumCallFrequency);
            SetCacheValue("CacheSet", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a double value in the cache with the given name and time to live, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Double value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <param name="MaximumCallFrequency">Maximum number of set calls per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetDouble(string Key, double Value, double CacheTime, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheWrite, MaximumCallFrequency);
            SetCacheValue("CacheSetDouble", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save an integer value in the cache with the given name and time to live, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Integer value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <param name="MaximumCallFrequency">Maximum number of set calls per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetInt(string Key, int Value, double CacheTime, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheWrite, MaximumCallFrequency);
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a boolean value in the cache with the given name and time to live, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Boolean value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <param name="MaximumCallFrequency">Maximum number of set calls per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetBool(string Key, bool Value, double CacheTime, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheWrite, MaximumCallFrequency);
            SetCacheValue("CacheSetBool", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a string value in the cache with the given name and time to live, restricting maximum call frequency to the range 2 to 1000 calls per second if required.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">String value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <param name="MaximumCallFrequency">Maximum number of set calls per second that are to be allowed for this item. A value of 0.0 will disable throttling. See <see cref="Cache.Dispose()"/> for further information.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <exception cref="InvalidValueException">When MaximumCallFrequency is not 0.0 and is outside the range 2.0 to 1000.0.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetString(string Key, string Value, double CacheTime, double MaximumCallFrequency)
        {
            Throttle(Key, CacheAction.CacheWrite, MaximumCallFrequency);
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        #endregion

        #region ICacheExtra implementation

        // Convenience overload members only accessible from .NET applications

        /// <summary>
        /// Immediatley retrieve an object from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested object, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public object Get(string Key)
        {
            return GetCacheValue("CacheGet", Key);
        }

        /// <summary>
        /// Immediatley retrieve a double value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested double value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public double GetDouble(string Key)
        {
            return (double)GetCacheValue("CacheGetDouble", Key);
        }

        /// <summary>
        /// Immediatley retrieve an integer value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested integer value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public int GetInt(string Key)
        {
            return (int)GetCacheValue("CacheGetInt", Key);
        }

        /// <summary>
        /// Immediatley retrieve a boolean value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested boolean value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public bool GetBool(string Key)
        {
            return (bool)GetCacheValue("CacheGetBool", Key);
        }

        /// <summary>
        /// Immediatley retrieve a string value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested string value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="NotInCacheException">When there is no item in the cache with the supplied key.</exception>
        public string GetString(string Key)
        {
            return (string)GetCacheValue("CacheGetString", Key);
        }

        /// <summary>
        /// Save an object in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Object to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void Set(string Key, object Value, double CacheTime)
        {
            SetCacheValue("CacheSet", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a double value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Double value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetDouble(string Key, double Value, double CacheTime)
        {
            SetCacheValue("CacheSetDouble", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save an integer value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Integer value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetInt(string Key, int Value, double CacheTime)
        {
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a boolean value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Boolean value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetBool(string Key, bool Value, double CacheTime)
        {
            SetCacheValue("CacheSetBool", Key, Value, CacheTime);
        }

        /// <summary>
        /// Save a string value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">String value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <exception cref="InvalidValueException">When Key is null or empty.</exception>
        /// <exception cref="InvalidValueException">When CacheTime is negative.</exception>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetString(string Key, string Value, double CacheTime)
        {
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        #endregion

        #region Support code

        private void LogMessage(string Source, string Message)
        {
            if (TL != null)
            {
                try
                {
                    TL.LogMessageCrLf(Source, Message); // Log the message if a TraceLogger has been set
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("LogMessage", "Exception: " + ex.ToString()); // Log the message if a TraceLogger has been set
                }
            }
        }

        private void CacheUpdatedCallback(CacheEntryRemovedArguments arguments)
        {
            LogMessage("CacheUpdatedCallback", string.Format("Item removed from cache: {0} {1}, Reason: {2}", arguments.CacheItem.Key, arguments.CacheItem.Value.ToString(), arguments.RemovedReason.ToString()));
        }

        private object GetCacheValue(string Caller, string Key)
        {
            // Validate parameters
            if (string.IsNullOrEmpty(Key)) throw new InvalidValueException("The supplied ASCOM Cache key is null or empty when retrieving an item from the cache.");

            CacheItem item;

            try
            {
                syncMutex.WaitOne();
                item = memoryCache.GetCacheItem(Key); // Attempt to retrieve the cached item
                lastRead[Key] = DateTime.Now; // Save last read time
            }
            finally
            {
                syncMutex.ReleaseMutex();
            }

            if (item == null)
            {
                LogMessage(Caller, string.Format("Value for {0} is not cached.", Key));
                throw new NotInCacheException(string.Format("A value for key {0} is not cached.", Key));
            }

            LogMessage(Caller, string.Format("Retrieved Key {0} {1}", item.Key, item.Value.ToString()));
            return item.Value;

        }

        private void SetCacheValue(string Caller, string Key, object Value, double CacheTime)
        {
            // Validate parameters
            if (string.IsNullOrEmpty(Key)) throw new InvalidValueException("The supplied ASCOM Cache key is null or empty when adding an item to the cache.");

            if (double.IsNaN(CacheTime)) throw new InvalidValueException("The supplied ASCOM Cache item lifetime is invalid: Double.NaN.");
            if (double.IsPositiveInfinity(CacheTime)) throw new InvalidValueException("The supplied ASCOM Cache item lifetime is invalid: Double.PoitiveInfinity.");
            if (double.IsNegativeInfinity(CacheTime)) throw new InvalidValueException("The supplied ASCOM Cache item lifetime is invalid: Double.NegativeInfinity.");
            if (CacheTime < 0.0) throw new InvalidValueException(string.Format("The ASCOM Cache item lifetime must be zero or positive, supplied value is negative: {0}", CacheTime));

            DateTimeOffset nowPlusoffset = new DateTimeOffset(DateTime.Now.AddSeconds(CacheTime));

            if (TL != null) removedCallBack = CacheUpdatedCallback;
            else removedCallBack = null;

            CacheItemPolicy policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = nowPlusoffset,
                RemovedCallback = removedCallBack
            };

            try
            {
                syncMutex.WaitOne();
                memoryCache.Set(Key, Value, policy);
                lastWritten[Key] = DateTime.Now; // Save last read time
            }
            finally
            {
                syncMutex.ReleaseMutex();
            }

            lastRead.Remove(Key); // This does not throw an exception if the key is not present

            LogMessage(Caller, string.Format("Key {0} set to: {1} with timeout {2} seconds", Key, Value, CacheTime));

        }

        private void Throttle(string Key, CacheAction action, double CallFrequency)
        {
            double remainingMilliSeconds = 0.0, minimumTimeBeforeNextCall = 0.0;

            // Validate parameters
            if (string.IsNullOrEmpty(Key)) throw new InvalidValueException("Supplied ASCOM Cache key is null or empty when adding an item.");

            if (double.IsNaN(CallFrequency)) throw new InvalidValueException("Supplied ASCOM Cache maximum call frequency is invalid: Double.NaN.");
            if (CallFrequency < 0.0) throw new InvalidValueException(string.Format("Supplied ASCOM Cache maximum call frequency must be positive, supplied value is negative: {0}", CallFrequency));
            if ((CallFrequency > 0.0) & (CallFrequency < ALLOWABLE_THROTTLING_RATE_MINIMUM)) throw new InvalidValueException(string.Format("Supplied ASCOM Cache call frequency {0} is below the minimum supported value {1}", CallFrequency, ALLOWABLE_THROTTLING_RATE_MINIMUM));
            if (CallFrequency > ALLOWABLE_THROTTLING_RATE_MAXIMUM) throw new InvalidValueException(string.Format("Supplied ASCOM Cache call frequency {0} is above the maximum supported value {1}", CallFrequency, ALLOWABLE_THROTTLING_RATE_MAXIMUM));

            DateTime lastAction = DateTime.MinValue; // Initialise variable so that we don'get a compiler warning later

            if (CallFrequency > 0.0) // Zero is a special value that specifies no call throttling
            {
                // Throttling is required
                try
                {
                    switch (action)
                    {
                        case CacheAction.CacheRead:
                            lastAction = lastRead[Key]; // Get the time of the last read, if set. This will throw a KeyNotFoundException if the key is not present i.e. there has been no previous call.
                            break;
                        case CacheAction.CacheWrite:
                            lastAction = lastWritten[Key]; // Get the time of the last write, if set. This will throw a KeyNotFoundException if the key is not present i.e. there has been no previous call.
                            break;
                        default:
                            throw new InvalidValueException(string.Format("ASCOM Cache Throttle - internal error - supplied CacheAction was neither CacheRead nor CacheWrite: {0}", action.ToString()));
                    }

                    // If a "KeyNotFound exception was NOT generated above, this method has been called before and throttling is in effect, so we need to check whether sufficient time has 
                    // passed since the last call to allow this call to proceed immediately.

                    minimumTimeBeforeNextCall = 1.0 / CallFrequency; // Calculate the minimum time duration between calls (in seconds) from the maximum call frequency

                    TimeSpan timeFromLastCall = DateTime.Now.Subtract(lastAction); // Calculate how long it has been since the last Get method call and store as a TimeSpan value
                    if (timeFromLastCall.TotalSeconds <= minimumTimeBeforeNextCall) // Test whether sufficient time has passed to execute the next call immediately. If so then continue with no delay
                    {
                        // Insufficient time has passed, so calculate the delay required to ensure the minimum tiome has passed before executing the call
                        int delayMilliSeconds = Convert.ToInt32((1000.0 * minimumTimeBeforeNextCall) - timeFromLastCall.TotalMilliseconds);
                        LogMessage("CacheGetDouble", string.Format("Throttling Get {0} call for {1} milliseconds", Key, delayMilliSeconds));

                        if (delayMilliSeconds > 0)
                        {
                            // Insufficient time has passed since the previous call so we need to delay this call.
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            LogMessage("Throttle", string.Format("Waiting for {0} milliseconds, pumped message interval: {1}", delayMilliSeconds, pumpMessagesInterval));

                            // Set the highest timer reolution if not already set
                            lock (timerResolutionChangeLock) // Make sure only one instance of this code can run at a time
                            {
                                if (!timerResolutionHasBeenChecked) CheckAndSetTimerResolutionIfRequired(); // If the timer resolution has not already been checked then check it now
                            }

                            if (pumpMessagesInterval > 0) // We need to break up long waits and pump events intermittently to keep the UI responsive
                            {

                                DateTime startTime = DateTime.Now; // Save the wait's start time
                                if (delayMilliSeconds > pumpMessagesInterval) // There will be at least one PUMP_TIME Sleep / DoEvents loop
                                {
                                    do
                                    {
                                        Thread.Sleep(pumpMessagesInterval); // Have a short sleep
                                        Application.DoEvents(); // Keep the UI alive
                                    } while (DateTime.Now.Subtract(startTime).TotalMilliseconds < (double)(delayMilliSeconds - pumpMessagesInterval)); // Wait until there are less than PUMP_TIME milliseonds left
                                }

                                // Now sleep the remaining few milliseconds
                                remainingMilliSeconds = delayMilliSeconds - DateTime.Now.Subtract(startTime).TotalMilliseconds; // Calculate the remaining time in milliseconds
                                if (remainingMilliSeconds > timerResolutionCurrent)
                                {
                                    Thread.Sleep(Convert.ToInt32(remainingMilliSeconds)); // Sleep any outstanding milliseconds
                                }
                            }
                            else // No need to pump events so just go to sleep for the required time
                            {
                                Thread.Sleep(delayMilliSeconds);
                            }

                            sw.Stop();
                            LogMessage("Throttle", string.Format("OVERALL - Requested {0}ms wait, actual wait {1}ms", delayMilliSeconds, sw.ElapsedMilliseconds));
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    // Key doesn't exist so we can allow this read at full speed, no need to delay
                }
                catch (Exception ex)
                {
                    LogMessage("Throttle", "Exception: " + ex.ToString());
                }
            }
        }

        private void CheckAndSetTimerResolutionIfRequired()
        {
            TIMECAPS timeCapabilities = new TIMECAPS();
            int timerResolutionRequired;

            // This code is called from within a timer change lock statement so we can just carry on here without further checks on whether other instances are running
            timerResolutionHasBeenChecked = true; // Flag that we have been called so that we don't get called again

            // Get the minimum, maximum and current timer resolutions
            uint timeCapabilitiesErrorCode = TimeGetDevCaps(ref timeCapabilities, Marshal.SizeOf(typeof(TIMECAPS)));
            uint rc = NtQueryTimerResolution(out int minimumResolution, out int maximimResolution, out int currentResolution);
            currentResolution /= 10000; // Convert the result to integer milliseconds from hundreds of nanoseconds.

            // If there was no error getting the timer resolution information validate the supplied value and apply it
            if (timeCapabilitiesErrorCode == 0)
            {
                timerResolutionRequired = TIMER_RESOLUTION_REQUIRED; // Set the default new timer resolution
                if (TIMER_RESOLUTION_REQUIRED < timeCapabilities.wPeriodMin) timerResolutionRequired = timeCapabilities.wPeriodMin; // Revise the required timer resolution if the hardware can't support the default value
                if (TIMER_RESOLUTION_REQUIRED > timeCapabilities.wPeriodMax) timerResolutionRequired = timeCapabilities.wPeriodMax; // Revise the required timer resolution if the hardware can't support the default value

                if (timerResolutionRequired < currentResolution) // The requested period is shorter than the current period so set the new shorter time period
                {
                    LogMessage("CheckSetTimerResolution", string.Format("Setting time period to {0}ms, Minimum period: {1}ms, maximum period: {2}ms", timerResolutionRequired, timeCapabilities.wPeriodMin, timeCapabilities.wPeriodMax));
                    uint result = TimeBeginPeriod(timerResolutionRequired);
                    timerResolutionHasBeenChanged = true;
                    timerResolutionCurrent = timerResolutionRequired;
                }
                else
                {
                    LogMessage("CheckSetTimerResolution", string.Format("Requested period ({0}) is longer trhan current ({1}), ignoring request to change period", timerResolutionRequired, currentResolution));
                }
            }
            else
            {
                LogMessage("CheckSetTimerResolution", string.Format("There was an error getting the time capabilities - the time period wlil not be changed. Time capbilities error code: {0:X8}", timeCapabilitiesErrorCode));
            }
        }

        #endregion

        #region Timer DLL calls and structures
        private const string WINMM = "winmm.dll";
        private const string WINNT = "ntdll.dll";

        [DllImport(WINMM, ExactSpelling = true, EntryPoint = "timeGetDevCaps")]
        private static extern uint TimeGetDevCaps(ref TIMECAPS ptc, int cbtc);

        [DllImport(WINMM, ExactSpelling = true, EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(int uPeriod);

        [DllImport(WINMM, ExactSpelling = true, EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(int uPeriod);

        [DllImport(WINNT, ExactSpelling = true, SetLastError = true)]
        private static extern uint NtQueryTimerResolution(out int MinimumResolution, out int MaximumResolution, out int CurrentResolution);

        [StructLayout(LayoutKind.Sequential)]
        private struct TIMECAPS
        {
            internal int wPeriodMin;
            internal int wPeriodMax;
        }

        #endregion
    }

}
