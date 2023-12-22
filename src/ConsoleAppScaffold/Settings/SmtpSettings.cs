namespace ConsoleAppScaffold.Settings;

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; } = 25;
    public string FromAddress { get; set; }
    public List<string> CcAddresses { get; set; } = new();
    public List<string> BccAddresses { get; set; } = new();
}