Example of Subscribing to a WeakEvent
EventMgr.SubscribeToEvent(this, OnSetLanguage); 
where OnSetLanguage is an Action delegate method with the signature of void Method(StandardMessage)

Example of sending (publishing) a weak event
EventMgr.PublishEvent(new EventData("SetLanguage", null, LanguageName, Token));
The Token is used to uniquely identify a message in a multiple instance application environment, like web sessions.

Example of cleanup (unsubscribing) which should be used in the dispose method of any class with a subscription.
EventMgr.UnsubscribeAll(this);

Note: I use the EventData class as a generic payload for the message.  
It has sufficient properties for most messaging requirements.
When publishing, an instance of the EventData class is passed to all subscribers.

To add WeakEvents to a Blazor application component, you can follow these steps:

In Program.cs...
builder.Services.AddScoped<IEventMgr, EventMgr>();

In the page that will subscribe to the events

Add this to the razor file...
@inject IJSRuntime JSRuntime

Add this to the razor .cs file...
[Inject]
public required IEventMgr EventMgr { get; set; } // Added dependency injection for EventMgr

Add the method that will handle the event when raised...
void OnOpenLink(object sender, StandardMessage m)   //must have this signature except for method name
{
    EventData eventData = m.Value as EventData;  //you need this to extract the EventData object

    if (eventData is not null && !string.IsNullOrEmpty(eventData.Data.ToString()))
    {
        _ = OpenAsync(eventData.Data.ToString(), eventData.Args.ToString() ?? "");
    }
}

In the component that will raise the event...

@inject IEventMgr EventMgr

OnClick = '()=> EventMgr.PublishEvent(new EventData(this, args object, data object))'