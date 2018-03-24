using System.Collections.Specialized;

namespace lorem
{
    internal class IpsumPostData : NameValueCollection
    {
        public IpsumPostData(Options options) : base()
        {
            Add("amount", options.Amount.ToString());
            Add("what", WhatOption(options));
            Add("start", StartOption(options));
            Add("generate", "Generate Lorem Ipsum");
        }

        private static string StartOption(Options options)
        {
            return options.NoStart ? "no" : "yes";
        }

        private static string WhatOption(Options options)
        {
            if (options.Words)
                return "words";
            if (options.Bytes)
                return "bytes";
            return "paras";
        }
    }
}
