namespace atprotosharp;

public class AccountInviteCodesResult
{
    public List<Code> codes { get; set; }

    public class Code
    {
        public string code { get; set; }
        public int available { get; set; }
        public bool disabled { get; set; }
        public string forAccount { get; set; }
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public List<Use> uses { get; set; }
    }
    public class Use
    {
        public string usedBy { get; set; }
        public DateTime usedAt { get; set; }
    }
}

