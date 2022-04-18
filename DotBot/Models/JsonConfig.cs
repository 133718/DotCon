namespace DotBot.Models
{
    internal class JsonConfig
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ulong DiscordId { get; set; }
        public char Prefix { get; set; }
    }
}
