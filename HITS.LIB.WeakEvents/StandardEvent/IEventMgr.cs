using CommunityToolkit.Mvvm.Messaging;

namespace HITS.LIB.WeakEvents
{
    public interface IEventMgr
    {
        void PublishEvent(EventData eventData);
        void SubscribeToEvent(object subscriber, MessageHandler<object, StandardMessage> method);
        void UnsubscribeAll(object subscriber);
    }
}