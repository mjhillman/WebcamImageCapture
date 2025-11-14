using CommunityToolkit.Mvvm.Messaging;

namespace HITS.LIB.WeakEvents
{
    public interface ILinkEventMgr
    {
        void PublishEvent(LinkData linkData);
        void SubscribeToEvent(object subscriber, MessageHandler<object, LinkMessage> method);
        void UnsubscribeAll(object subscriber);
    }
}