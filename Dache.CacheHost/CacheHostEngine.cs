﻿using System;
using System.ServiceModel;
using Dache.CacheHost.Storage;
using Dache.Communication;
using Dache.Core.Interfaces;

namespace Dache.CacheHost
{
    /// <summary>
    /// The engine which runs the cache host.
    /// </summary>
    public class CacheHostEngine : IRunnable
    {
        // The cache server
        private readonly IRunnable _cacheServer = null;
        // The cache host information poller
        private readonly IRunnable _cacheHostInformationPoller = null;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="cacheHostInformationPoller">The cache host information poller.</param>
        /// <param name="memCache">The mem cache to use for storing objects.</param>
        /// <param name="cacheServer">The cache server.</param>
        public CacheHostEngine(IRunnable cacheHostInformationPoller, MemCache memCache, IRunnable cacheServer)
        {
            // Sanitize
            if (cacheHostInformationPoller == null)
            {
                throw new ArgumentNullException("cacheHostInformationPoller");
            }
            if (memCache == null)
            {
                throw new ArgumentNullException("memCache");
            }
            if (cacheServer == null)
            {
                throw new ArgumentNullException("cacheServer");
            }

            // Set the cache host information poller
            _cacheHostInformationPoller = cacheHostInformationPoller;

            // Set the mem cache container instance
            MemCacheContainer.Instance = memCache;

            // Initialize the serer
            _cacheServer = cacheServer;
        }

        /// <summary>
        /// Starts the cache host engine.
        /// </summary>
        public void Start()
        {
            // Begin listening for requests
            _cacheServer.Start();

            // Start the cache host information poller
            _cacheHostInformationPoller.Start();
        }

        /// <summary>
        /// Stops the cache host engine.
        /// </summary>
        public void Stop()
        {
            // Dispose the MemCache guaranteed
            using (MemCacheContainer.Instance)
            {
                // Stop listening for requests
                _cacheServer.Stop();

                // Stop the cache host information poller
                _cacheHostInformationPoller.Stop();
            }
        }
    }
}
