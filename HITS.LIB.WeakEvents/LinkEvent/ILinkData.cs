namespace HITS.LIB.WeakEvents
{
    public interface ILinkData
    {
        string Description { get; set; }
        string Href { get; set; }
        object Sender { get; set; }
        string Target { get; set; }
        string Token { get; set; }
    }
}