using AngleSharp.Parser.Html;
using CommandLine;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace lorem
{
    class Program
    {
        const string lipsumUrl = "https://www.lipsum.com/feed/html";

        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Ipsum(options));
        }

        private static void Ipsum(Options options)
        {
            NameValueCollection postData = GetPostData(options);
            string result = GetIpsum(postData, options);
            if (options.Console)
                Console.WriteLine(result);
            else
                Clipboard.SetText(result);
        }

        private static string GetIpsum(NameValueCollection postData, Options options)
        {
            string result;
            HtmlParser parser = new HtmlParser();
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(lipsumUrl, "POST", postData);
                result = Encoding.UTF8.GetString(response);
            }
            var doc = parser.Parse(result);
            var paragraphs = doc.QuerySelectorAll("div#lipsum p");

            var output = new StringBuilder();
            foreach (var p in paragraphs)
                output.Append(GetParagraph(p.TextContent, options.NoHtml, options.WrapLength));

            return output.ToString();
        }

        private static StringBuilder GetParagraph(string paragraphText, bool noHtml, int wrapLength)
        {
            var paragraph = new StringBuilder();
            if (!noHtml)
                paragraph.Append("<p>");

            if (wrapLength == 0)
                paragraph.Append(paragraphText);
            else
                paragraph.Append(WrapText(paragraphText, wrapLength));

            if (!noHtml)
                paragraph.Append("</p>\n");

            return paragraph;
        }

        private static StringBuilder WrapText(string paragraphText, int wrapLength)
        {
            var wrapped = new StringBuilder();
            string[] words = paragraphText.Split(' ');
            int length = 0;
            foreach (string word in words)
            {
                if (length + word.Length > wrapLength)
                {
                    wrapped.Append('\n');
                    length = 0;
                }
                wrapped.Append(word);
                wrapped.Append(' ');
                length += word.Length + 1;
            }
            --wrapped.Length;
            return wrapped;
        }

        private static NameValueCollection GetPostData(Options options)
        {
            var postData = new NameValueCollection();
            postData.Add("amount", options.Amount.ToString());
            postData.Add("what", whatOption(options));
            postData.Add("start", startOption(options));
            postData.Add("generate", "Generate Lorem Ipsum");
            return postData;
        }

        private static string startOption(Options options)
        {
            return options.NoStart ? "no" : "yes";
        }

        private static string whatOption(Options options)
        {
            if (options.Words)
                return "words";
            if (options.Bytes)
                return "bytes";
            return "paras";
        }
    }
}
