using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        /// <summary>
        /// 指定したﾌｫﾙﾀﾞ内の階層構造を一つのﾌｫﾙﾀﾞに纏めます。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //var src = "123test456test789";
            //var result1 = Regex.Replace(src, @"\d+", p => string.Format("{0,0:D6}", int.Parse(p.Value)));
            //var result2 = Regex.Replace(src, @"\d+", p => p.Value.PadLeft(6, '0'));
            //Console.WriteLine($"SourceString={src}\nResult1={result1}\nResult2={result2}");
            //var s = "123test456test7890123";
            //Console.WriteLine(Regex.Replace(s, @"\d+", p => p.Value.PadLeft(6, '0')));
            //Console.WriteLine(Regex.Replace(Regex.Replace(s, @"\d+", "00000$0"), @"(0*)(\d{5})", "$2"));
            //Console.WriteLine(string.Format("{0,0:D6}", 123));
            foreach (var path in args)
            {
                Execute(path);
            }
            Console.ReadLine();
        }

        private static void Execute(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("ﾌｫﾙﾀﾞが見つからないので処理を終了します。");
                return;
            }

            Console.WriteLine($"{path}");
            Console.WriteLine("処理を開始します。");

            // ﾘﾈｰﾑ処理実行
            Rename(path, path);

            // ﾌｧｲﾙ名整形
            Organize(path);

            Console.WriteLine("処理が完了しました。");
        }

        private static void Rename(string baseDir, string target)
        {
            foreach (var file in Directory.GetFiles(target))
            {
                var identifer = baseDir.Equals(target)
                    ? "__"
                    : Directory.GetParent(file).Name;

                identifer = "color".Equals(identifer)
                    ? "_"
                    : identifer;

                File.Move(
                    file,
                    Path.Combine(baseDir, $"{identifer}{Path.GetFileName(file)}")
                );
            }
            
            foreach (var dir in Directory.GetDirectories(target))
            {
                Rename(baseDir, dir);
            }

            if (!baseDir.Equals(target))
            {
                Directory.Delete(target);
            }
        }

        private static void Organize(string path)
        {
            Directory.GetFiles(path)
                .Select(file => Path.GetFileName(file))
                .Select(file =>
                {
                    var src = Path.Combine(path, file);
                    var dst = Path.Combine(path, Regex.Replace(file, @"\d+", p => p.Value.PadLeft(6, '0')));

                    File.Move(src, dst);
                    
                    return dst;
                })
                .OrderBy(s => s)
                .Select((file, index) =>
                {
                    var extension = Path.GetExtension(file);
                    var fileindex = string.Format("{0,0:D5}", index + 1);
                    var filename = fileindex + extension;

                    File.Move(
                        Path.Combine(path, file),
                        Path.Combine(path, filename)
                    );
                    return file;
                })
                .ToArray();
        }
    }
}
