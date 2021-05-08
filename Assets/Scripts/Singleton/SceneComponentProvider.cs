using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

namespace Singleton
{
    /*
     * Класс, позволяющий не пробрасывать в сцене ссылки на кучу компонентов 
     * 
     * Автоматически становится бессмертным при первом создании. Это важно для починки 
     * вот такого кейса:
     * 1. Сцена выгружается, Unity убивает SceneComponentProvider
     * 2. Другой компонент в OnDestroy пытается от чего-то отписаться (получить через
     *    SceneComponentProvider что-то) => создаётся новый SceneComponentProvider
     * 3. Новый SceneComponentProvider не получает эвентов о смене сцен и содержит в кэше
     *    неактуальные объекты.
     */
    public class SceneComponentProvider : Singleton<SceneComponentProvider>
    {
        private Dictionary<Type, Object> objectCache;
        private bool isDestroying = false;
		[SerializeField] Component[] presetObjects;

        private static bool IsAllowedToDoIt
        {
            get
            {
                return !applicationIsQuitting
                    && Instance != null
                    && !Instance.isDestroying;
            }
        }

        protected override void Awake()
		{
            // Делаем свой объект бессмертным (причины в комменте выше)
            var go = gameObject;
            while (go.transform.parent != null) go = go.transform.parent.gameObject;
            DontDestroyOnLoad(go);

			base.Awake();
            objectCache = new Dictionary<Type, Object>();

            if (presetObjects != null)
			{
				for (int i = 0; i < presetObjects.Length; i++)
				{
					if (presetObjects[i] == null) continue;
					objectCache[presetObjects[i].GetType()] = presetObjects[i];
				}
            }

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene _)
        {
            isDestroying = false;
        }

        public override void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            base.OnDestroy();
        }

        public static void RegisterComponent(Type t, Object o)
        {
            if(IsAllowedToDoIt)
                Instance.objectCache[t] = o;
        }

        public new static Object GetComponent(Type t)
        {
            if (!IsAllowedToDoIt)
                return null;

            // проверяем непосредственно тип в кэше
            if (Instance.objectCache.ContainsKey(t))
            {
                return Instance.objectCache[t];
            }
            // проевряем, вдруг в кэше есть объект более конкретного типа
            foreach (var ct in Instance.objectCache.Keys)
            {
                if (t.IsAssignableFrom(ct))
                {
                    // на будущее запоминаем, что по базовому типу также
					// надо выдавать этот же объект
					Instance.objectCache[t] = Instance.objectCache[ct];
					return Instance.objectCache[ct];
                }
            }
			// Кто-то может запросить тупо класс или даже интерфейс.
			// В таком случае они должны быть зареганы заранее, искать их
			// через FindObjectsOfType бессмысленно.
			if (!t.IsSubclassOf(typeof(MonoBehaviour))) return null;
            var objects = FindObjectsOfType(t);
			if (objects == null) return null;
            if (objects.Length > 1)
            {
				// Если объект именно запрошенного типа только один, выдадим его и 
				// запомним (поможет от случаев, когда на сцене есть два объекта, являющиеся
				// предком и потомком в иерархии.
				int total = 0;
				Object any = null;
				for (var i = 0; i < objects.Length; i++)
				{
					if (objects[i].GetType() == t)
					{
						total++;
						any = objects[i];
					}
				}
				if (total == 1)
				{
					Instance.objectCache[t] = any;
					return any;
				}
                throw new Exception("SceneComponentProvider unable to provide " + t.ToString() 
                                    + ": we have " + objects.Length.ToString() + " objects");
            }
            if (objects.Length == 0) return null;
            Instance.objectCache[t] = objects[0];
            return objects[0];
        }
    }
}