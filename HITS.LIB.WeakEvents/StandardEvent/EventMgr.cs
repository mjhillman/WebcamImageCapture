using CommunityToolkit.Mvvm.Messaging;

namespace HITS.LIB.WeakEvents
{
    /// <summary>
    /// Event Manager for handling events using WeakReferenceMessenger
    /// </summary>
    public class EventMgr : IEventMgr
    {
        public EventMgr()
        {
        }

        public void PublishEvent(EventData eventData)
        {
            WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
        }

        public void SubscribeToEvent(object subscriber, MessageHandler<object, StandardMessage> method)
        {
            WeakReferenceMessenger.Default.Register(subscriber, handler: method);
        }

        public void UnsubscribeAll(object subscriber)
        {
            WeakReferenceMessenger.Default.UnregisterAll(subscriber);
        }
    }
}
