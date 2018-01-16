namespace Todo.Model
{
    public class EmailMessageDT
    {
        public string FromEmail { get; set;}
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool IsImportant { get; set; }
    }
}