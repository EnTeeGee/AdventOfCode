namespace AdventOfCodeCore.Core
{
    internal interface IRunner
    {
        void PrintStartupMessage();

        bool CheckRequest(string[] args);
    }
}
