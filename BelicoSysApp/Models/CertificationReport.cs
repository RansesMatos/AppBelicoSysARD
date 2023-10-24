namespace BelicoSysApp.Models
{
    public class CertificationReport
    {
        public string CertifierName { get; set; }
        public string CertifierTitle { get; set; }
        public string CertificationDate { get; set; }
        public List<string> WeaponDetails { get; set; }
        // Add other fields as required.
    }
}
