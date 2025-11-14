using CommunityToolkit.Mvvm.Messaging;

namespace HITS.LIB.WeakEvents
{
    /// <summary>
    /// Event Manager for handling "<a" tag events using WeakReferenceMessenger
    /// </summary>
    public class LinkEventMgr : ILinkEventMgr
    {
        public LinkEventMgr()
        {
        }

        public void PublishEvent(LinkData linkData)
        {
            WeakReferenceMessenger.Default.Send(new LinkMessage(linkData));
        }

        public void SubscribeToEvent(object subscriber, MessageHandler<object, LinkMessage> method)
        {
            WeakReferenceMessenger.Default.Register(subscriber, handler: method);
        }

        public void UnsubscribeAll(object subscriber)
        {
            WeakReferenceMessenger.Default.UnregisterAll(subscriber);
        }
    }
}
