using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Services.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly Dictionary<Type, object> _registeredService;

        public static ServiceRepository Default = new ServiceRepository();

        public ServiceRepository()
        {
            _registeredService = new Dictionary<Type, object>();
        }

        public T GetService<T>() where T : class
        {
            Type type = typeof(T);

            if (_registeredService.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            return default;
        }

        public void AddService<T>(T service) where T : class
        {
            Type type = typeof(T);

            if (_registeredService.ContainsKey(type))
            {
                throw new ArgumentException($"Service of type {nameof(type)} is already registered");
            }

            _registeredService.Add(type, service);
        }

        public void RemoveService<T>(T service) where T : class
        {
            Type type = typeof(T);

            _registeredService.Remove(type);
        }
    }
}
