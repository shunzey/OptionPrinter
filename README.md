# OptionPrinter

An option usage printer for CUI command line programs.
The option format is based on Unix style.
Options should be defined with short name like `-n` and long name like `--name` that must be required.

# Sample code

```C#
using System;
using OptionPrintService;

namespace ConsoleStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Usage: testprog [OPTIONS]...");
            OptionPrinter printer = new OptionPrinter();
            printer.Add("-h", "--help", "Print usage");
            printer.Add("-v", "--version", "Show the version of this software");
            printer.Add(null, "--log logfile", "Set a file path for master log");
            printer.Add(null, "--window", "Show a gui window for debugging");

            Console.Write(printer.Print());
            Console.WriteLine("--------");

            // Usage: testprog [OPTIONS]...
            //   -h, --help          Print usage
            //   -v, --version       Show the version of this software
            //       --log logfile   Set a file path for master log
            //       --window        Show a gui window for debugging
            // --------
        }
    }
}
```