using TextCopy;

namespace AdventOfCodeCore.Core
{
    static internal class Clipboard
    {
        public static string? Read()
        {
            return ClipboardService.GetText()?.Trim();
        }

        public static void Write(string data)
        {
            ClipboardService.SetText(data);
        }
    }
}
