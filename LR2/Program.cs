public class Test {

    public static void Main(String[] args) {

        Board board = new Board("25037460");
        Console.WriteLine(board);
        
        Console.WriteLine("RBFS");

        DateTime start = DateTime.Now;

        RBFS.algorithm(board);
        Console.WriteLine("Time " + (DateTime.Now - start).TotalSeconds);

        Console.WriteLine("BFS");

        start = DateTime.Now;

        BFS.algorithm(board);
        Console.WriteLine("Time " + (DateTime.Now - start).TotalSeconds);
        
    }
}

public class Board:IComparable
{
    public int[,] indexes;

    public Board() {
    }
    

    public Board(String state) {
        int size = state.Length;
        indexes = new int[size,size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++)
                indexes[i,j] = 0;
        }

        for (int i = 0; i < size; i++) {
            indexes[Int32.Parse(state[i].ToString()),i] = 1;
        }
    }

    public int[,] Copy(Board brd)
    {
        indexes = new int[brd.indexes.GetLength(0), brd.indexes.GetLength(1)];
        for (int i = 0; i < indexes.GetLength(0); i++)
        {
            for (int j = 0; j < indexes.GetLength(1); j++)
            {
                indexes[i, j] = brd.indexes[i,j];
            }
        }
        return indexes;
    }

    public override string ToString()
    {
        string line = "";
        for (int i = 0; i < indexes.GetLength(0); i++)
        {
            for (int j = 0; j < indexes.GetLength(1); j++)
            {
                line +=indexes[i,j];
            }

            line += Environment.NewLine;
        }

        return line;
    }

    public bool IsSafe() {
        for (int i = 0; i < indexes.GetLength(0); i++) {
            for (int j = 0; j < indexes.GetLength(1); j++) {
                if (indexes[i,j] == 1) {
                    if (!checkPosition(i, j)) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool checkPosition(int i, int j) {
        for (int k = j + 1; k < indexes.GetLength(1); k++) {
            if (indexes[i,k] == 1) {
                return false;
            }
        }
        
        int n;
        int m;
        
        for (n = i + 1, m = j + 1; n < indexes.GetLength(0) && m < indexes.GetLength(1); n++, m++) {
            if (indexes[n,m] == 1) {
                return false;
            }
        }

        for (n = i + 1, m = j - 1; n < indexes.GetLength(0) && m >= 0; n++, m--) {
            if (indexes[n,m] == 1) {
                return false;
            }
        }
        return true;
    }

    public int getHeuristic() {
        int heuristic = 0;
        
        for (int i = 0; i < indexes.GetLength(0); i++) {
            for (int j = 0; j < indexes.GetLength(1); j++) {
                if (indexes[i,j] == 1) {
                    int n;
                    int m;

                    for (n = i + 1, m = j + 1; n < indexes.GetLength(0) && m < indexes.GetLength(1); n++, m++) {
                        if (indexes[n,m] == 1) {
                            heuristic++;
                        }
                    }

                    for (n = i + 1, m = j - 1; n < indexes.GetLength(0) && m >= 0; n++, m--) {
                        if (indexes[n,m] == 1) {
                            heuristic++;
                        }
                    }

                    for (n = i - 1, m = j + 1; n >= 0 && m < indexes.GetLength(1) ; n--, m++) {
                        if (indexes[n,m] == 1) {
                            heuristic++;
                        }
                    }

                    for (n = i - 1, m = j - 1; n >= 0 && m >= 0 ; n--, m--) {
                        if (indexes[n,m] == 1) {
                            heuristic++;
                        }
                    }

                    for (m = j + 1; m < indexes.GetLength(1); m++) {
                        if (indexes[i,m] == 1) {
                            heuristic++;
                        }
                    }

                    for (m = j - 1; m >= 0 ; m--) {
                        if (indexes[i,m] == 1) {
                            heuristic++;
                        }
                    }

                }
                
            }
        }
        return heuristic;
    }

    public int CompareTo(object? obj)
    {
        Board? brd = (Board)obj!;
        if (getHeuristic() == brd.getHeuristic())
            return 0;
        {
            if (getHeuristic() >  brd.getHeuristic())
                return 1;
            return -1;
        }
    }
    
}




public class BFS {
    private static long counterOfStatesBFS = 0;
    private static long iterationsBFS = 0;

    public BFS() {
    }

    public static void algorithm(Board board)
    {
        Queue<Board> queue = new Queue<Board>();
        HashSet<Board> visited = new HashSet<Board>();

        queue.Enqueue(board);
        visited.Add(board);

        Board current = new Board();
        

        bool flag = queue.Peek().IsSafe();
        current.indexes = current.Copy(queue.Peek());


        while (!flag) {
            iterationsBFS++;
            flag = queue.Peek().IsSafe();
            current.indexes = current.Copy(queue.Peek());
            visited.Add(queue.Peek());
            generateNextStateBFS(queue, queue.Dequeue(), visited);
        }

        Console.WriteLine("Answer\n" + current);
        Console.WriteLine("iterations " + iterationsBFS);
        Console.WriteLine("counterOfStates " + counterOfStatesBFS);
       Console.WriteLine("queue.size() " + queue.Count());

    }

    public static void generateNextStateBFS(Queue<Board> queue, Board board, HashSet<Board> visited) {
        Board newBoard = new Board();
        newBoard.indexes = newBoard.Copy(board);

        for (int i = 0; i < board.indexes.GetLength(0); i++) {
            for (int j = 0; j < board.indexes.GetLength(1); j++) {
                if (board.indexes[j,i] == 1) {
                    newBoard.indexes[j,i] = 0;
                    for (int k = 0; k < board.indexes.GetLength(1); k++) {
                        if (k != j) {
                            newBoard.indexes[k,i] = 1;
                            counterOfStatesBFS++;

                            Board state = new Board();
                            state.indexes = state.Copy(newBoard);
                            if (!queue.Contains(state) || !visited.Contains(state)) {
                                queue.Enqueue(state);
                            }
                            newBoard.indexes[k,i] = 0;
                        }

                    }
                    newBoard.indexes[j,i] = 1;
                }
            }
        }
    }

}

public class RBFS
{
    private static long counterOfStatesRBFS = 0;
    private static long iterationsRBFS = 0;

    public static void algorithm(Board board)
    {
        HashSet<Board> visited = new HashSet<Board>();

        Board current = new Board();

        bool flag = board.IsSafe();
        current.indexes = current.Copy(board);

        recursiveRBFS(board, visited, current);

        Console.WriteLine("Answer\n" + current);
        Console.WriteLine("iterations " + iterationsRBFS);
        Console.WriteLine("counterOfStates " + counterOfStatesRBFS);
    }

    private static void recursiveRBFS(Board board, HashSet<Board> visited, Board current)
    {
        iterationsRBFS++;
        visited.Add(board);

        if (board.IsSafe())
        {
            current.indexes = current.Copy(board);
            return;
        }

        List<Board> successors = generateSuccessorsRBFS(board, visited);

        foreach (var successor in successors)
        {
            counterOfStatesRBFS++;
            if (!visited.Contains(successor))
            {
                recursiveRBFS(successor, visited, current);
                if (current.IsSafe())
                {
                    return;
                }
            }
        }
    }

    private static List<Board> generateSuccessorsRBFS(Board board, HashSet<Board> visited)
    {
        List<Board> successors = new List<Board>();

        for (int i = 0; i < board.indexes.GetLength(0); i++)
        {
            for (int j = 0; j < board.indexes.GetLength(1); j++)
            {
                if (board.indexes[j, i] == 1)
                {
                    Board newBoard = new Board();
                    newBoard.indexes = newBoard.Copy(board);

                    newBoard.indexes[j, i] = 0;

                    for (int k = 0; k < board.indexes.GetLength(1); k++)
                    {
                        if (k != j)
                        {
                            newBoard.indexes[k, i] = 1;

                            Board state = new Board();
                            state.indexes = state.Copy(newBoard);

                            if (!visited.Contains(state))
                            {
                                successors.Add(state);
                            }

                            newBoard.indexes[k, i] = 0;
                        }
                    }
                    newBoard.indexes[j, i] = 1;
                }
            }
        }
        successors.Sort((x, y) => x.getHeuristic().CompareTo(y.getHeuristic()));
        return successors;
    }
}


