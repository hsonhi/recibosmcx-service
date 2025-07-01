namespace DeCrypto
{
    public class PDFSignerType
    {
        private static string MXExpress = "noreply@mcxexpress.co.ao";

        public static string Signer(string signer)
        {
            return signer.Contains(MXExpress) ? "MCX Express" :
                signer;
        }
    }
}
