using System;
using System.Collections.Generic;

public static class EventManager {
    private static Dictionary<string, Action<object[]>> events = new();

    public static void Subscribe(string eventName, Action<object[]> listener) {
        if (!events.ContainsKey(eventName))
            events[eventName] = delegate { };
        events[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<object[]> listener) {
        if (events.ContainsKey(eventName))
            events[eventName] -= listener;
    }

    public static void Trigger(string eventName, params object[] args) {
        if (events.ContainsKey(eventName))
            events[eventName].Invoke(args);
    }
}