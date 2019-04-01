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
        /// 削除対象のﾌｧｲﾙ拡張子
        /// </summary>
        private static string[] IgnoreExtension = new string[]
        {
            ".db",
            ".dll",
            ".htm",
            ".lnk",
            ".url",
            ".html",
            ".shtml",
            ".txt"
        };

        /// <summary>
        /// 削除対象のﾃﾞｨﾚｸﾄﾘ名
        /// </summary>
        private static string[] IgnoreDirectory = new string[]
        {
            "単ページ"
        };

        /// <summary>
        /// 指定したﾌｫﾙﾀﾞ内の階層構造を一つのﾌｫﾙﾀﾞに纏めます。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                Execute(path);
            }
        }

        /// <summary>
        /// 指定したﾃﾞｨﾚｸﾄﾘの処理を実行します。
        /// </summary>
        /// <param name="path">ﾃﾞｨﾚｸﾄﾘのﾌﾙﾊﾟｽ</param>
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

        /// <summary>
        /// ﾘﾈｰﾑ処理を実行します。
        /// </summary>
        /// <param name="baseDir">基底ﾃﾞｨﾚｸﾄﾘ</param>
        /// <param name="target">対象ﾃﾞｨﾚｸﾄﾘ</param>
        private static void Rename(string baseDir, string target)
        {
            if (!IgnoreDirectory.Any(dir => target.ToLower().EndsWith(dir)))
            {
                foreach (var file in Directory.GetFiles(target))
                {
                    if (!IgnoreExtension.Any(extension => file.ToLower().EndsWith(extension)))
                    {
                        // 処理対象
                        var identifer = baseDir.Equals(target)
                            ? "__"
                            : Directory.GetParent(file).Name;

                        identifer = "color".Equals(identifer)
                            ? "_"
                            : identifer;

                        var dest = Path.Combine(baseDir, $"{identifer}{Path.GetFileName(file)}");

                        if (File.Exists(dest)) File.Delete(dest);

                        File.Move(file,  dest);
                    }
                    else
                    {
                        // 削除対象
                        File.Delete(file);
                    }
                }

                foreach (var dir in Directory.GetDirectories(target))
                {
                    Rename(baseDir, dir);
                }
            }

            if (!baseDir.Equals(target))
            {
                DeleteAll(target);
            }
        }

        /// <summary>
        /// ﾃﾞｨﾚｸﾄﾘ内のﾌｧｲﾙを連番に変更します。
        /// </summary>
        /// <param name="path">ﾃﾞｨﾚｸﾄﾘ</param>
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

        /// <summary>
        /// ﾃﾞｨﾚｸﾄﾘを削除します。
        /// </summary>
        /// <param name="path">ﾃﾞｨﾚｸﾄﾘ</param>
        private static void DeleteAll(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                // ﾌｧｲﾙを削除
                File.Delete(file);
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                // 子ﾃﾞｨﾚｸﾄﾘの内容を削除
                DeleteAll(dir);
            }

            // ﾃﾞｨﾚｸﾄﾘを削除
            Directory.Delete(path);
        }
    }
}
