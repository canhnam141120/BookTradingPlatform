namespace BTP_API.Helpers
{
    public class EnumVariable
    {
        public enum StorageStatus { Waiting, Received, Sent, Recall, Refund };
        public enum Status { Waiting, Trading, Complete, Cancel };

        public enum StatusRequest { Waiting, Approved, Denied, Cancel };

        public enum Fees { S1, S13, S35, S5, B1, BM };
    }
}
