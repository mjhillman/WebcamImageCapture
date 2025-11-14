using CommunityToolkit.Mvvm.Messaging.Messages;

namespace HITS.LIB.WeakEvents
{
    /// <summary>
    /// This class defines a message
    /// Define the LinkMessage class inheriting from ValueChangedMessage
    /// In the MVVM Toolkit, ValueChangedMessage<T> is a generic class used for messaging purposes. 
    /// It allows you to send a message that encapsulates a value that has changed, 
    /// making it easy to notify other parts of your application about the change.
    /// Key Features:
    /// Generic Type(T): The type parameter T represents the type of the value that has changed.In this case, it's ILinkData.
    /// Constructor: The class has a constructor that accepts the changed value(LinkData) and initializes the message with it.
    /// Value Property: It provides a property to access the value that has changed.
    /// How It Works:
    /// When a value changes, you create an instance of ValueChangedMessage<T> with the updated value.
    /// This message can then be sent using the IMessenger interface provided by the MVVM Toolkit.
    /// Other components (e.g., ViewModels or services) can subscribe to these messages and respond accordingly.
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/messenger</remarks>
    public class LinkMessage : ValueChangedMessage<ILinkData>
    {
        // Constructor for LinkMessage, accepting ILinkData as a parameter
        public LinkMessage(ILinkData linkData) : base(linkData)
        {
            // Additional functionality or initialization can be added here
        }
    }
}
