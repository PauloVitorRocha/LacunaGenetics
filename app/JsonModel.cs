public class JSONModel
{
    public class ResponseObject
    {
        public Job job { get; set; } = new Job();
        public string code { get; set; } = "";
        public string message { get; set; } = "";

    }
    public class Response
    {
        public string code { get; set; } = "";
        public string message { get; set; } = "";
        public string accessToken { get; set; } = "";
    }
}
