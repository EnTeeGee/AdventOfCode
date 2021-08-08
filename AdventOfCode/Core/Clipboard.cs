using System;
using System.Threading;

namespace AdventOfCode.Core
{
    static class Clipboard
    {
        public static string Read()
        {
            var text = (string)null;

            var clipboardAction = new Action(() =>
            {
                if (!System.Windows.Clipboard.ContainsText())
                {
                    text = string.Empty;
                }

                text = System.Windows.Clipboard.GetText();
            });

            var thread = new Thread(new ThreadStart(clipboardAction));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(TimeSpan.FromSeconds(5));

            return text;
        }

        public static void Write(string data)
        {
            var clipboardAction = new Action(() =>
            {
                System.Windows.Clipboard.SetText(data);
                Console.WriteLine("Result copied to clipboard.");
            });

            var thread = new Thread(new ThreadStart(clipboardAction));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
