using Microsoft.Extensions.Configuration;
using System;
using Todo.Library;

namespace Todo.Core
{
    public static class Constants
    {
        static readonly IConfiguration config = BL.Instance.Configuration;

        public static readonly string GmailCredentialUsername = config["EmailProvider:GmailCredentialUsername"];
        public static readonly string GmailCredentialPassword = config["EmailProvider:GmailCredentialPassword"];
        public static readonly string GmailHost = config["EmailProvider:GmailHost"];
        public static readonly int GmailPort = Convert.ToInt32(config["EmailProvider:GmailPort"]);
        public static readonly string AzureFunctionApi = config["AzureFunction:API"];
        public static readonly string AzureFunctionUrl = config["AzureFunction:Url"];
    }
}
