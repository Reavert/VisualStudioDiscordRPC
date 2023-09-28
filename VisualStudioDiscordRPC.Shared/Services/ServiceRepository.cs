using System;
using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class ServiceRepository
    {
        private readonly Dictionary<Type, object> _registeredService;

        public readonly static ServiceRepository Default = new ServiceRepository();

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
                throw new ArgumentException($"Service of type {type.FullName} is already registered");
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
