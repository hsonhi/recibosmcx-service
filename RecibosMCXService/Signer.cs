namespace RecibosMCXService
{
    public class Signer
    {
        public bool Signature { get; set; }
        public int Revision { get; set; }
        public int TotalRevisions { get; set; }
        public bool IntegrityCheck { get; set; }
        public string? DigestAlgorithm { get; set; }
        public string? EncryptionAlgorithm { get; set; }
        public string? Info { get; set; }
        public string? Date { get; set; }
        public string? Timezone { get; set; }
        //public string? Location { get; set; }
        //public string? Reason { get; set; }
        //public string? Contact { get; set; }
        public string? Output { get; set; }
    }
}