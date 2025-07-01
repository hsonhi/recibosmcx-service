namespace DeCrypto
{
    public class PDFSignatureInfo
    {
        public bool SignatureCoverWholeDoc { get; set; }
        public int DocRevision { get; set; }
        public int DocTotalRevisions { get; set; }
        public bool IntegrityCheck { get; set; }
        public string DigestAlgorithm { get; set; }
        public string EncryptionAlgorithm { get; set; }
        public string Signer { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public string Contact { get; set; }
        public string Error { get; set; }
    }
}