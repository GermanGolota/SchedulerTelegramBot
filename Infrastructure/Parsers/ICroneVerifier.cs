namespace Infrastructure.Parsers
{
    public interface ICroneVerifier
    {
        bool VerifyCron(string cron);
    }
}