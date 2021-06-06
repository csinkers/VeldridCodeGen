﻿using System;
using UAlbion.Api;

namespace UAlbion.Core
{
    public abstract class Handler
    {
        public Type Type { get; }
        public IComponent Component { get; }
        protected Handler(Type type, IComponent component)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Component = component ?? throw new ArgumentNullException(nameof(component));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="continuation"></param>
        /// <returns>True if the handler intends to call, or has already called the continuation.</returns>
        public abstract bool Invoke(IEvent e, Action<object> continuation);
        public override string ToString() => $"H<{Component.GetType().Name}, {Type.Name}>";
    }

    public class Handler<TEvent> : Handler
    {
        public Action<TEvent> Callback { get; }
        public Handler(Action<TEvent> callback, IComponent component) : base(typeof(TEvent), component) => Callback = callback;
        public override bool Invoke(IEvent e, Action<object> _) { Callback((TEvent) e); return false; }
    }

    public class AsyncHandler<TEvent> : Handler
    {
        public Func<TEvent, Action, bool> Callback { get; }
        public AsyncHandler(Func<TEvent, Action, bool> callback, IComponent component) : base(typeof(TEvent), component) => Callback = callback;
        public override bool Invoke(IEvent e, Action<object> continuation) => Callback((TEvent)e, () => continuation(null));
    }

    public class AsyncHandler<TEvent, TReturn> : Handler
    {
        public Func<TEvent, Action<TReturn>, bool> Callback { get; }
        public AsyncHandler(Func<TEvent, Action<TReturn>, bool> callback, IComponent component) : base(typeof(TEvent), component) => Callback = callback;
        public override bool Invoke(IEvent e, Action<object> continuation) => Callback((TEvent)e, x => continuation(x));
    }
}