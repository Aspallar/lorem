using CommandLine;

namespace lorem
{
    internal class Options
    {
        [Value(0, Default = 1, HelpText = "Amount")]
        public int Amount { get; set; }

        [Option('w', HelpText = "Words")]
        public bool Words { get; set; }

        [Option('b', HelpText = "Bytes")]
        public bool Bytes { get; set; }

        [Option('n', HelpText = "Don't start with \"Lorum Ipsum\"")]
        public bool NoStart { get; set; }

        [Option('l', Default = 80, HelpText = "Length to word wrap at (0 = no wrap)")]
        public int WrapLength { get; set; }

        [Option('h', HelpText = "No html (no <p> tags)")]
        public bool NoHtml { get; set; }

        [Option('c', HelpText = "Output to console instead of clipboard")]
        public bool Console { get; set; }
    }
}
