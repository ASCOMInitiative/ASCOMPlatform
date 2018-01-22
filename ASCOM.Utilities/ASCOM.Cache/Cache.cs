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
    /// ASCOM caching component
    /// </summary>
    [Guid("01057AB6-76D6-4161-8D5E-992E6E1CD97C")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Cache : ICache, ICacheExtra
    {
        private const string MEMORY_CACHE_NAME = "ASCOM";
        private const int PUMP_MESSAGES_INTERVAL_DEFAULT = 0;

        private MemoryCache memoryCache;
        private TraceLogger TL;
        private Dictionary<string, DateTime> lastRead;
        private CacheEntryRemovedCallback removedCallBack;
        private int pumpMessagesInterval;

        private Mutex syncMutex;

        #region Initialisers

        /// <summary>
        /// Default initialiser that does not assign a TraceLogger unless the cache tracing flag is set.
        /// </summary>
        /// <remarks>COM (as opposed to .NET) applications must use this initialiser because parameterised initialisers can not be used by COM applications.
        /// If the cache tracing flag is set a separate trace log will be produced by each instance of the cache.</remarks>
        public Cache()
        {
            memoryCache = new MemoryCache(MEMORY_CACHE_NAME);
            lastRead = new Dictionary<string, DateTime>();
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
        /// <remarks>The initialiser will accept null (Nothing) as a valid reference, this will suppress all cache logging.</remarks>
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

        #endregion

        #region ICache implementation

        // Members accessible from both COM and .NET applications

        /// <summary>
        /// Get or set the interval, in milliseconds, between calls to Application.DoEvents to pump Windows messages when throttling the cache read rate.
        /// </summary>
        /// <remarks>Setting this property to zero will prevent any message pumping by the cache while waiting. The default value of the property is zero.</remarks>
        public int PumpMessagesInterval
        {
            get
            {
                return pumpMessagesInterval;
            }
            set
            {
                pumpMessagesInterval = value;
                LogMessage("PumpMessagesInterval", string.Format("Messages set to be pumped every {0} milliseconds", pumpMessagesInterval));
            }
        }

        /// <summary>
        /// Save an object in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Object to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void Set(string Key, object Value, double CacheTime)
        {
            SetCacheValue("CacheSet", Key, Value, CacheTime);
        }

        /// <summary>
        /// Retrieve an object from the cache with the given name. Call frequency will be restricted to the supplied rate by delaying execution as necessary.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item.</param>
        /// <returns>The requested object if present in the cache or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public object Get(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, MaximumCallFrequency);
            return Get(Key); //Get the value
        }

        /// <summary>
        /// Save a double value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Double value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetDouble(string Key, double Value, double CacheTime)
        {
            SetCacheValue("CacheSetDouble", Key, Value, CacheTime);
        }

        /// <summary>
        /// Retrieve a double value from the cache with the given name. Call frequency will be restricted to the supplied rate by delaying execution as necessary.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item.</param>
        /// <returns>The requested double value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public double GetDouble(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, MaximumCallFrequency);
            return GetDouble(Key); //Get the value
        }

        /// <summary>
        /// Save an integer value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Integer value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetInt(string Key, int Value, double CacheTime)
        {
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        /// <summary>
        /// Retrieve an integer value from the cache with the given name. Call frequency will be restricted to the supplied rate by delaying execution as necessary.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item.</param>
        /// <returns>The requested integer value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public int GetInt(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, MaximumCallFrequency);
            return GetInt(Key); //Get the value
        }

        /// <summary>
        /// Save a string value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">String value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetString(string Key, string Value, double CacheTime)
        {
            SetCacheValue("CacheSetInt", Key, Value, CacheTime);
        }

        /// <summary>
        /// Retrieve a string value from the cache with the given name. Call frequency will be restricted to the supplied rate by delaying execution as necessary.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item.</param>
        /// <returns>The requested string value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public string GetString(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, MaximumCallFrequency);
            return GetString(Key); //Get the value
        }

        /// <summary>
        /// Save a boolean value in the cache with the given name and time to live.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="Value">Boolean value to be stored in the cache.</param>
        /// <param name="CacheTime">Time in seconds before the item will be automatically removed from the cache.</param>
        /// <remarks>Any existing item in the cache with the same name will be overwritten.</remarks>
        public void SetBool(string Key, bool Value, double CacheTime)
        {
            SetCacheValue("CacheSetBool", Key, Value, CacheTime);
        }

        /// <summary>
        /// Retrieve a boolean value from the cache with the given name. Call frequency will be restricted to the supplied rate by delaying execution as necessary.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <param name="MaximumCallFrequency">Maximum number of retrievals per second that are to be allowed for this item.</param>
        /// <returns>The requested boolean value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public bool GetBool(string Key, double MaximumCallFrequency)
        {
            Throttle(Key, MaximumCallFrequency);
            return GetBool(Key); //Get the value
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
        /// <remarks>No exception is thrown if the specified item does not exist.</remarks>
        public void Remove(string Key)
        {
            object item;

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

        #endregion

        #region ICacheExtra implementation

        // Members only accessible from .NET applications

        /// <summary>
        /// Immediatley retrieve a double value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested double value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public double GetDouble(string Key)
        {
            return (double)GetCacheValue("CacheGetDouble", Key);
        }

        /// <summary>
        /// Immediatley retrieve an integer value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested integer value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public int GetInt(string Key)
        {
            return (int)GetCacheValue("CacheGetInt", Key);
        }

        /// <summary>
        /// Immediatley retrieve an object from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested object, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public object Get(string Key)
        {
            return GetCacheValue("CacheGet", Key);
        }

        /// <summary>
        /// Immediatley retrieve a string value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested string value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public string GetString(string Key)
        {
            return (string)GetCacheValue("CacheGetString", Key);
        }

        /// <summary>
        /// Immediatley retrieve a boolean value from the cache with the given name with no throttling.
        /// </summary>
        /// <param name="Key">Name of this item in the cache. The key is case sensitive.</param>
        /// <returns>The requested boolean value, if present in the cache, or a "NotInCacheException" if there is no item in the cache that has the supplied name.</returns>
        /// <exception cref="NotInCacheException">Thrown if there is no item in the cache with the supplied key.</exception>
        public bool GetBool(string Key)
        {
            return (bool)GetCacheValue("CacheGetBool", Key);
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

        private void WaitFor(int duration)
        {
            if (pumpMessagesInterval > 0) // We need to break up long waits and pump events intermittently to keep the UI responsive
            {
                DateTime startTime = DateTime.Now; // Save the wait's start time
                if (duration > pumpMessagesInterval) // There will be at least one PUMP_TIME Sleep / DoEvents loop
                {
                    do
                    {
                        Thread.Sleep(pumpMessagesInterval); // Have a short sleep
                        Application.DoEvents(); // Keep the UI alive
                        //LogMessage("WaitFor", string.Format("Pumped messages, time to go: {0} milliseconds ", (duration - pumpMessagesInterval) - DateTime.Now.Subtract(startTime).TotalMilliseconds ));
                    } while (DateTime.Now.Subtract(startTime).TotalMilliseconds < (double)(duration - pumpMessagesInterval)); // Wait until there are less than PUMP_TIME milliseonds left
                }

                // Now sleep the remaining few milliseconds
                int remainingMilliSeconds = (int)(duration - DateTime.Now.Subtract(startTime).TotalMilliseconds); // Calculate the remaining time in milliseconds
                //LogMessage("WaitFor", string.Format("Remaining milliseconds: {0}", remainingMilliSeconds));
                if (remainingMilliSeconds > 0)
                {
                    LogMessage("WaitFor", string.Format("Waiting for remaining {0} milliseconds", remainingMilliSeconds));
                    Thread.Sleep(remainingMilliSeconds); // Sleep any outstanding milliseconds
                }
            }
            else // No need to pump events so just go to sleep for the required time
            {
                LogMessage("WaitFor", string.Format("Not pumping, just waiting for {0} milliseconds", duration));
                Thread.Sleep(duration);
            }
        }

        private object GetCacheValue(string Caller, string Key)
        {
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
            }
            finally
            {
                syncMutex.ReleaseMutex();
            }

            lastRead.Remove(Key); // This does not throw an exception if the key is not present

            LogMessage(Caller, string.Format("Key {0} set to: {1} with timeout {2} seconds", Key, Value, CacheTime));

        }

        private void Throttle(string Key, double CallMaximumCallFrequency)
        {
            if (CallMaximumCallFrequency > 0) // Zero is a special value that specifies no call throttling
            {
                try
                {

                    DateTime last = lastRead[Key]; // Get the time of the last call, if set. This will throw a KeyNotFoundException if the key is not present i.e. there has been no previous call.
                    double minimumTimeBeforeNextCall = 1.0 / CallMaximumCallFrequency; // Calculate the minimum time duration between calls (in seconds) from the maximum call frequency

                    // If we are here this method has been called before and throttling is in effect, so we need to check whether sufficient time has 
                    // passed since the last call to allow this call to proceed immediately.

                    TimeSpan timeFromLastCall = DateTime.Now.Subtract(last); // Calculate how long it has been since the last Get method call and store as a TimeSpan value
                    if (timeFromLastCall.TotalSeconds <= minimumTimeBeforeNextCall) // Test whether sufficient time has passed to execute the next call immediately.
                    {
                        // Insufficient time has passed, so calculate the delay required to ensure the minimum tiome has passed before executing the call
                        int delayMilliSeconds = Convert.ToInt32((1000.0 * minimumTimeBeforeNextCall) - timeFromLastCall.TotalMilliseconds);
                        LogMessage("CacheGetDouble", string.Format("Throttling Get {0} call for {1} milliseconds", Key, delayMilliSeconds));
                        WaitFor(delayMilliSeconds);
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

        #endregion
    }

}
