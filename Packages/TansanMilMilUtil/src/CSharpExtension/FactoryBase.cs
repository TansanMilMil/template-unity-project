using System;

namespace TansanMilMil.Util
{
    public abstract class FactoryBase<T, S> : IFactory<T, S>
    {
        public abstract T Create(S id);
    }
}