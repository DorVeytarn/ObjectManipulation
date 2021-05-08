using System;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class ComponentProvider : Singleton<ComponentProvider>
    {
        private static readonly Dictionary<Type, object> _cachedComponents = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, object> _cachedObjects = new Dictionary<Type, object>();

        protected ComponentProvider() { } // guarantee this will be always a singleton only - can't use the constructor!

        #region PUBLIC METHODS
            
        /// <summary>
        ///  (optional) allow runtime registration of global objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object RegisterInSingleScope(Type T)
        {
            if (!T.IsSubclassOf(typeof(MonoBehaviour)))
            {
                throw new Exception("You should register non-monobehaviours by creating in compile-time");
            }
            else
            {
                return GetOrAddComponent(T);
            }
        }

        /// <summary>
        ///  (optional) allow runtime registration of global objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object RegisterComponent(Type T, object targetObject)
        {
            if (!T.IsSubclassOf(typeof(MonoBehaviour)))
            {
                return AddObject(T, targetObject);
            }
            else
            {
                return AddComponent(T, targetObject);
            }
        }
        
        /// <summary>
        /// return provider component if exists or null
        /// </summary>
        public static object GetProviderComponent(Type T)
        {
            if (T.IsSubclassOf(typeof(MonoBehaviour)))
            {
                var foundComponent = GetCachedComponent(T);
                if (foundComponent != null) return foundComponent;
                var inst = Instance;
                return inst == null ? null : inst.GetComponent(T);
            }
            else
            {
                if (_cachedObjects.ContainsKey(T)) return _cachedObjects[T];
                // check for subclasses
                foreach (var t in _cachedObjects.Keys)
                {
                    if (T.IsAssignableFrom(t))
                    {
                        return _cachedObjects[t];
                    }
                }
                return null;
            }
        }

		/// <summary>
		/// returns component on ComponentProvider itself or searches for it
		/// on scene. You should not use this on non-immortal objects, 
		/// use SceneComponentProvider for that
		/// </summary>
		public static object GetOrFindComponent(Type t)
		{
			if (_cachedComponents.ContainsKey(t))
				return _cachedComponents[t];
			var objects = FindObjectsOfType(t);
			if (objects.Length > 1)
			{
				throw new Exception("ComponentProvider unable to provide " + t.ToString()
									+ ": we have " + objects.Length.ToString() + " objects");
			}
			if (objects.Length == 0) return null;
			_cachedComponents[t] = objects[0];
			return objects[0];
		}

		#endregion

		#region private methods

		/// <summary>
		/// Gets or add a component.
		/// </summary>
		private static object GetOrAddComponent(Type T)
        {
            if (_cachedComponents.ContainsKey(T))
                return _cachedComponents[T];
            var inst = Instance;
            var target = inst.GetComponent(T) ?? inst.gameObject.AddComponent(T);
            _cachedComponents.Add(T, target);
            return target;
        }

        /// <summary>
        /// Gets or add a component.
        /// </summary>
        private static object AddComponent(Type T, object component)
        {
            if (component == null) return null;
            var foundObject = GetCachedComponent(T);
            if (foundObject != null) return foundObject;
            _cachedComponents.Add(T, component);
            return component;
        }

        private static object GetCachedComponent(Type T)
        {
            if (_cachedComponents.ContainsKey(T))
                return _cachedComponents[T];
            return null;
        }

        private static object AddObject(Type T, object o)
        {
            _cachedObjects[T] = o;
            return o;
        }
        #endregion
    }
}
