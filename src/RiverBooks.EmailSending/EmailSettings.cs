namespace RiverBooks.EmailSending;

public class EmailSettings
{
  public string SmtpServer { get; set; } = "localhost";
  public int SmtpPort { get; set; } = 25;
}
