using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day09
    {
        [Solution(9, 1)]
        public long Solution1(string input)
        {
            var fileList = new LinkedList<FileNode>();
            for(var i = 0; i < input.Length; i+= 2)
            {
                fileList.AddLast(new FileNode(i / 2, int.Parse(input[i].ToString())));
                if(i + 1 < input.Length)
                    fileList.AddLast(new FileNode(-1, int.Parse(input[i + 1].ToString())));
            }

            var currentSpace = fileList.First!.Next;

            while(currentSpace != null && currentSpace.Value.Id == -1)
            {
                while (fileList.Last!.Value.Id == -1)
                    fileList.RemoveLast();

                var currentToMove = fileList.Last;

                if (currentToMove.Value.Length == currentSpace.Value.Length)
                {
                    currentSpace.Value = currentToMove.Value;
                    currentSpace = currentSpace.Next?.Next;
                    fileList.RemoveLast();
                }
                else if (currentToMove.Value.Length < currentSpace.Value.Length)
                {
                    fileList.AddBefore(currentSpace, currentToMove.Value);
                    currentSpace.Value = new FileNode(-1, currentSpace.Value.Length - currentToMove.Value.Length);
                    fileList.RemoveLast();
                }
                else
                {
                    currentSpace.Value = new FileNode(currentToMove.Value.Id, currentSpace.Value.Length);
                    currentToMove.Value = new FileNode(currentToMove.Value.Id, currentToMove.Value.Length - currentSpace.Value.Length);
                    currentSpace = currentSpace.Next?.Next;
                }
            }

            var output = 0L;
            var currentIndex = 0;
            foreach(var item in fileList)
            {
                output += item.GetChecksum(currentIndex);
                currentIndex += item.Length;
            }

            return output;
        }

        [Solution(9, 2)]
        public long Solution2(string input)
        {
            var fileList = new LinkedList<FileNode>();
            for (var i = 0; i < input.Length; i += 2)
            {
                fileList.AddLast(new FileNode(i / 2, int.Parse(input[i].ToString())));
                if (i + 1 < input.Length)
                    fileList.AddLast(new FileNode(-1, int.Parse(input[i + 1].ToString())));
            }

            var currentToMove = fileList.Last;

            while(currentToMove != null)
            {
                while (currentToMove!.Value.Id == -1)
                    currentToMove = currentToMove.Previous;

                var currentSpace = fileList.First;

                while(currentSpace != currentToMove)
                {
                    if(currentSpace!.Value.Id != -1 || currentSpace.Value.Length < currentToMove.Value.Length)
                    {
                        currentSpace = currentSpace.Next;
                        continue;
                    }

                    fileList.AddBefore(currentSpace, currentToMove.Value);
                    currentSpace.Value = new FileNode(-1, currentSpace.Value.Length - currentToMove.Value.Length);
                    if(currentSpace.Value.Length == 0)
                        fileList.Remove(currentSpace);

                    currentToMove.Value = new FileNode(-1, currentToMove.Value.Length);

                    break;
                }

                currentToMove = currentToMove.Previous;
            }

            var output = 0L;
            var currentIndex = 0;
            foreach (var item in fileList)
            {
                output += item.GetChecksum(currentIndex);
                currentIndex += item.Length;
            }

            return output;
        }

        private class FileNode
        {
            public int Id { get; }
            public int Length { get; }

            public FileNode(int id, int length)
            {
                Id = id;
                Length = length;
            }

            public long GetChecksum(int index)
            {
                if (Id == -1)
                    return 0;

                return Enumerable.Range(index, Length).Sum(it => it * (long)Id);
            }
        }
    }
}
