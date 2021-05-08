namespace Singleton
{
    /// <summary>
    /// D.I<T> = Dependency.Injection of type T.
    /// Обёртка для ComponentProvider для уменьшения количества кода.
    /// </summary>
    public static class D
    {
        /// <summary>
        /// Возвращает бессмертный компонент типа T.
        /// </summary>
        public static T I<T>() where T : class
        {
            return ComponentProvider.GetProviderComponent(typeof(T)) as T;
        }
    }
}
