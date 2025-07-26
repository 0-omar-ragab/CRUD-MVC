namespace IKEA.PL.Settings
{
    public class MilSettings
    {
        public string SenderEmail { get; set; } =null!;
        public string SenderPassword { get; set; } = null!;
        public string SmtpClientServer { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public int SmtpClientPort { get; set; }
    }
}
