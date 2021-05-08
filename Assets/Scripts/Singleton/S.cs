using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// S.DI<T> = Scene.DependencyInjection of type T.
    /// Обёртка над SceneComponentProvider для уменьшения количества кода.
    /// </summary>
    public static class S
    {
        /// <summary>
        /// Возвращает компонент на текущей сцене по типу.
        /// </summary>
        public static T DI<T>() where T : MonoBehaviour
        {
            return SceneComponentProvider.GetComponent(typeof(T)) as T;
        }
    }
}
