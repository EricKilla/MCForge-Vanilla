﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class OnPhysicsUpdateEvent
    {
        internal static List<OnPhysicsUpdateEvent> events = new List<OnPhysicsUpdateEvent>();
        Plugin plugin;
        Level.OnPhysicsUpdate method;
        Priority priority;
        internal OnPhysicsUpdateEvent(Level.OnPhysicsUpdate method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        public static void Call(ushort x, ushort y, ushort z, byte time, string extra, Level l)
        {
            events.ForEach(delegate(OnPhysicsUpdateEvent p1)
            {
                try
                {
                    p1.method(x, y, z, time, extra, l);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the LevelUnload Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<OnPhysicsUpdateEvent> temp = new List<OnPhysicsUpdateEvent>();
            List<OnPhysicsUpdateEvent> temp2 = events;
            OnPhysicsUpdateEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (OnPhysicsUpdateEvent p in temp2)
                {
                    if (temp3 == null)
                        temp3 = p;
                    else if (temp3.priority < p.priority)
                        temp3 = p;
                }
                temp.Add(temp3);
                temp2.Remove(temp3);
                temp3 = null;
                i++;
            }
            events = temp;
        }
        /// <summary>
        /// Find a event
        /// </summary>
        /// <param name="plugin">The plugin that registered this event</param>
        /// <returns>The event</returns>
        public static OnPhysicsUpdateEvent Find(Plugin plugin)
        {
            foreach (OnPhysicsUpdateEvent p in events.ToArray())
            {
                if (p.plugin == plugin)
                    return p;
            }
            return null;
        }
        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">This is the delegate that will get called when this event occurs</param>
        /// <param name="priority">The priority (imporantce) of this call</param>
        /// <param name="plugin">The plugin object that is registering the event</param>
        public static void Register(Level.OnPhysicsUpdate method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new OnPhysicsUpdateEvent(method, priority, plugin));
            Organize();
        }
        /// <summary>
        /// UnRegister this event
        /// </summary>
        /// <param name="plugin">The plugin object that has this event registered</param>
        public static void UnRegister(Plugin plugin)
        {
            if (Find(plugin) == null)
                throw new Exception("This plugin doesnt have this event registered!");
            else
                events.Remove(Find(plugin));
        }
    }
}
