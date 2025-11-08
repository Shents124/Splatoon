using System.Collections.Generic;
using ZBase.Foundation.PubSub;

namespace Runtime.PubSub
{
    public static class WorldMessengerExtension
    {
        public static void AddTo(this ISubscription subscription, List<ISubscription> subscriptions)
        {
            subscriptions.Add(subscription);
        }
        
        public static void UnsubscribeAll(this List<ISubscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Unsubscribe();
            }
            subscriptions.Clear();
        }
    }
}