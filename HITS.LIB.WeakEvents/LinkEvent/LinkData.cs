namespace HITS.LIB.WeakEvents
{
    public sealed class LinkData : IDisposable, ILinkData
    {
        public object Sender { get; set; }
        public string Href { get; set; }
        public string Target { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }

        public LinkData()
        {
        }

        public LinkData(object sender, string href)
        {
            Sender = sender;
            Href = href;
        }

        public LinkData(object sender, string href, string target)
        {
            Sender = sender;
            Href = href;
            Target = target;
        }

        public void Dispose()
        {
            if (Sender is IDisposable disposable) disposable.Dispose();
        }
    }
}
