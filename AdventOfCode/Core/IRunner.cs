namespace AdventOfCode.Core
{
    interface IRunner
    {
        void PrintStartupMessage();

        bool CheckRequest(string[] args);
    }
}
