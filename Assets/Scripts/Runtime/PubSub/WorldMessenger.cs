using System;
using Runtime.PubSub.Constant;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Runtime.PubSub
{
    public static class WorldMessenger
    {
        public static MessagePublisher Publisher => Instance.MessagePublisher;

        public static MessageSubscriber Subscriber => Instance.MessageSubscriber;
        
        public static AnonPublisher AnonPublisher => Instance.AnonPublisher;

        public static AnonSubscriber AnonSubscriber => Instance.AnonSubscriber;

        private static Messenger Instance => s_instance ??= new();

        private static Messenger s_instance;

#if UNITY_EDITOR
        // Hỗ trợ khi tắt Domain Reload trên editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_instance?.Dispose();
            s_instance = null;
        }
#endif
        public static ISubscription Sub<T> (Action action, int order = MessageConstant.DEFAULT_ORDER) where T : struct
        {
            
            return Instance.MessageSubscriber.Global().Subscribe<T>(action, order: order);
        }
        
        public static ISubscription Sub<T> (Action<T> action, int order = MessageConstant.DEFAULT_ORDER) where T : struct
        {
            return Instance.MessageSubscriber.Global().Subscribe<T>(action, order: order);
        }
        
        public static void Pub<T>(T message)
        {
            Instance.MessagePublisher.Global().Publish(message);
        }
        
        public static ISubscription Sub<TScope, TMessage>(Action action, int order = MessageConstant.DEFAULT_ORDER) 
            where TScope : struct
            where TMessage : struct
        {
            return Instance.MessageSubscriber.Scope<TScope>().Subscribe<TMessage>(action, order: order);
        }
        
        public static ISubscription Sub<TScope, TMessage>(Action<TMessage> action, int order = MessageConstant.DEFAULT_ORDER) 
            where TScope : struct
            where TMessage : struct
        {
            return Instance.MessageSubscriber.Scope<TScope>().Subscribe(action, order: order);
        }
        
        public static void Pub<TScope, TMessage>(TMessage message) 
            where TScope : struct
            where TMessage : struct
        {
            Instance.MessagePublisher.Scope<TScope>().Publish(message);
        }
    }
}