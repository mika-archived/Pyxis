namespace Pyxis.Beta.Events
{
    public class ReAuthenticateEventArgs
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }

    public delegate void ReAuthenticateEventHandler(ReAuthenticateEventArgs args);
}