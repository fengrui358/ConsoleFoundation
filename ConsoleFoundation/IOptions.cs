using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace ConsoleFoundation
{
    interface IOptions
    {
        [Option('n', "lines",
            Default = 5U,
            SetName = "bylines",
            HelpText = "Lines to be printed from the beginning or end of the file.")]
        uint? Lines { get; set; }

        [Option('c', "bytes",
            SetName = "bybytes",
            HelpText = "Bytes to be printed from the beginning or end of the file.")]
        uint? Bytes { get; set; }

        [Option('q', "quiet",
            HelpText = "Suppresses summary messages.")]
        bool Quiet { get; set; }

        [Value(0, MetaName = "input file",
            HelpText = "Input file to be processed.",
            Required = true)]
        string FileName { get; set; }

        [Option('p', "print", SetName = "printSetName", MetaValue = "printMetaValue", HelpText = "打印多行文本")]
        IEnumerable<string> Prints { get; set; }
    }

    /// <summary>
    /// 子命令需要加入Verb，如果是单独的命令可不需要
    /// </summary>
    [Verb("head", HelpText = "Displays first lines of a file.")]
    class HeadOptions : IOptions
    {
        public uint? Lines { get; set; }

        public uint? Bytes { get; set; }

        public bool Quiet { get; set; }

        public string FileName { get; set; }
        public IEnumerable<string> Prints { get; set; }

        [Usage(ApplicationAlias = "ReadText.Demo.exe")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("normal scenario", new HeadOptions { FileName = "file.bin" });
                yield return new Example("specify bytes", new HeadOptions { FileName = "file.bin", Bytes = 100 });
                yield return new Example("suppress summary", UnParserSettings.WithGroupSwitchesOnly(), new HeadOptions { FileName = "file.bin", Quiet = true });
                yield return new Example("read more lines", new[] { UnParserSettings.WithGroupSwitchesOnly(), UnParserSettings.WithUseEqualTokenOnly() }, new HeadOptions { FileName = "file.bin", Lines = 10 });
            }
        }
    }

    [Verb("tail", HelpText = "Displays last lines of a file.")]
    class TailOptions : IOptions
    {
        public uint? Lines { get; set; }

        public uint? Bytes { get; set; }

        public bool Quiet { get; set; }

        public string FileName { get; set; }
        public IEnumerable<string> Prints { get; set; }

        public IEnumerable<string> Files { get; set; }
    }
}