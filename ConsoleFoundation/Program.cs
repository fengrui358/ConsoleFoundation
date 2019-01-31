using System;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;

namespace ConsoleFoundation
{
    /// <summary>
    /// 控制台程序的基础框架
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            string Reader(IOptions opts)
            {
                var fromTop = opts.GetType() == typeof(HeadOptions);
                return opts.Lines.HasValue
                    ? ReadLines(opts.FileName, fromTop, (int) opts.Lines)
                    : ReadBytes(opts.FileName, fromTop, (int) opts.Bytes);
            }

            string Header(IOptions opts)
            {
                if (opts.Quiet)
                {
                    return string.Empty;
                }

                var fromTop = opts.GetType() == typeof(HeadOptions);

                if (opts.Prints != null && opts.Prints.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    Console.WriteLine("文件列表：");
                    foreach (var info in opts.Prints)
                    {
                        Console.WriteLine(info);
                    }

                    Console.ResetColor();
                }

                var builder = new StringBuilder("Reading ");
                builder = opts.Lines.HasValue
                    ? builder.Append(opts.Lines).Append(" lines")
                    : builder.Append(opts.Bytes).Append(" bytes");
                builder = fromTop ? builder.Append(" from top:") : builder.Append(" from bottom:");
                return builder.ToString();
            }

            void PrintIfNotEmpty(string text)
            {
                if (text.Length == 0)
                {
                    return;
                }

                Console.WriteLine(text);
            }

            #region 多项子命令

            var result = Parser.Default.ParseArguments<HeadOptions, TailOptions>(args);
            var texts = result
                .MapResult(
                    (HeadOptions opts) => Tuple.Create(Header(opts), Reader(opts)),
                    (TailOptions opts) => Tuple.Create(Header(opts), Reader(opts)),
                    _ => MakeError());

            #endregion

            #region 单命令

            //var result = Parser.Default.ParseArguments<HeadOptions>(args);
            //var texts = result
            //    .MapResult(
            //        (HeadOptions opts) => Tuple.Create(Header(opts), Reader(opts)),
            //        _ => MakeError());

            #endregion

            PrintIfNotEmpty(texts.Item1);
            PrintIfNotEmpty(texts.Item2);

            return texts.Equals(MakeError()) ? 1 : 0;
        }

        private static string ReadLines(string fileName, bool fromTop, int count)
        {
            var lines = File.ReadAllLines(fileName);
            if (fromTop)
            {
                return string.Join(Environment.NewLine, lines.Take(count));
            }
            return string.Join(Environment.NewLine, lines.Reverse().Take(count));
        }

        private static string ReadBytes(string fileName, bool fromTop, int count)
        {
            var bytes = File.ReadAllBytes(fileName);
            if (fromTop)
            {
                return Encoding.UTF8.GetString(bytes, 0, count);
            }
            return Encoding.UTF8.GetString(bytes, bytes.Length - count, count);
        }

        private static Tuple<string, string> MakeError()
        {
            return Tuple.Create("\0", "\0");
        }
    }
}
