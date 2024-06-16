using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Shared.Events
{
    public class EventBus
    {
        private static EventBus instance;
        private List<EventListenerWrapper> eventListeners = new List<EventListenerWrapper>();
        public static EventBus Instance { get { return instance ??= new EventBus(); } }

        private EventBus() { }

        public void Register(object listener)
        {
            if (!eventListeners.Any(l => l.Listener == listener))
                eventListeners.Add(new EventListenerWrapper(listener));
        }

        public void Unregister(object listener)
        {
            eventListeners.RemoveAll(l => l.Listener == listener);
        }

        public void UnregisterAll()
        {
            eventListeners.Clear();
        }

        public void PostEvent(object e)
        {
            eventListeners.Where(l => l.EventType.Contains(e.GetType())).ToList().ForEach(l => l.PostEvent(e));
        }

        public void PostEventGroup(object[] evs)
        {
            foreach (var e in evs)
            {
                PostEvent(e);
            }
        }

        private class EventListenerWrapper
        {
            public object Listener { get; private set; }
            public Type[] EventType { get; private set; }

            private MethodBase[] methods;

            public EventListenerWrapper(object listener)
            {
                Listener = listener;

                Type type = listener.GetType();

                methods = type
                    .GetMethods()
                    .Where(x => x.Name == "OnEvent").ToArray();

                if (methods.Length == 0)
                {
                    throw new ArgumentException("Class " + type.Name + " does not containt method OnEvent");
                }

                EventType = new Type[methods.Length];
                for (int i = 0; i < methods.Length; i++)
                {
                    ParameterInfo[] parameters = methods[i].GetParameters();

                    if (parameters.Length != 1)
                        throw new ArgumentException("Method OnEvent of class " + type.Name + " have invalid number of parameters (should be one)");

                    EventType[i] = parameters[0].ParameterType;
                }
            }

            public void PostEvent(object e)
            {
                for (int i = 0; i < EventType.Length; i++)
                {
                    if (e.GetType() == EventType[i])
                    {
                        methods[i].Invoke(Listener, new[] { e });
                    }
                }
            }
        }
    }
}