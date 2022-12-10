

class Day5 {

    public Day5() {}

    static void Main(string[] args) {
        string path = args[0];
        string[] input = File.ReadAllLines(path);

        int diagramEndIndex = FindFirstEmptyLine(input);

        string[] instructions = input.Skip(diagramEndIndex+1).ToArray();
        string[] diagram = input.Take(diagramEndIndex).ToArray();

        Graph graph = new Graph(diagram);

        int[,] processedInstructions = new int[instructions.Length,3];

        int instIndex = 0;
        foreach(string line in instructions) {
            int[] values = new int[3];
            int index = 0;
            // This would be easier with Regex, but it's 5AM and I don't want to use it
            foreach(char c in line) {
                if (char.IsWhiteSpace(c) && processedInstructions[instIndex, index] != 0) index++;
                if (char.IsDigit(c)) {
                    if (processedInstructions[instIndex, index] == 0)
                        processedInstructions[instIndex, index] = Int32.Parse(""+c);
                    else
                        processedInstructions[instIndex, index] = Int32.Parse(processedInstructions[instIndex, index] + (""+c));
                }
            }
            instIndex++;
        }
        Console.WriteLine("Start");
        foreach(KeyValuePair<int, List<char>> pair in graph.Stacks) {
            char[] clone = new char[pair.Value.Count()];
            pair.Value.CopyTo(clone);
            Console.WriteLine("Key: "+pair.Key);
            clone.Reverse();
            foreach(char c in clone) {
                Console.WriteLine("\t"+c);
            }
        }

        for(int i=0; i<processedInstructions.GetLength(0); i++) {
            Console.WriteLine("Instruction Number "+i);
            graph.MoveTopItem(processedInstructions[i,1], processedInstructions[i,2], processedInstructions[i,0]);
            foreach(KeyValuePair<int, List<char>> pair in graph.Stacks) {
                char[] clone = new char[pair.Value.Count()];
                pair.Value.CopyTo(clone);
                Console.WriteLine("Key: "+pair.Key);
                clone.Reverse();
                foreach(char c in clone) {
                    Console.WriteLine("\t"+c);
                }
            }
        }



        Console.WriteLine("Final");
        foreach(KeyValuePair<int, List<char>> pair in graph.Stacks) {
            char[] clone = new char[pair.Value.Count()];
            pair.Value.CopyTo(clone);
            Console.WriteLine("Key: "+pair.Key);
            clone.Reverse();
            foreach(char c in clone) {
                Console.WriteLine("\t"+c);
            }
        }

        for(int i=0; i<graph.Stacks.Count(); i++) {
            Console.WriteLine(graph.GetTopItem(i+1));
        }




    }

    static int FindFirstEmptyLine(string[] input) {
        int index = 0;
        foreach (string line in input) {
            if (string.IsNullOrEmpty(line)) {
                break;
            }
            index++;
        }
        return index;
    }


}

class Graph {
    public Dictionary<int, List<char>> Stacks;
    
    public Graph(string[] diagram) {
        Stacks = new Dictionary<int, List<char>>();
        string[] numbers = diagram[diagram.Length-1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int[] stackNumbers = new int[numbers.Length];

        int index = 0;
        foreach (string num in numbers) {
            stackNumbers[index] = Int32.Parse(num);
            index++;
        }

        int multiple = 1;
        foreach (string box in new ArraySegment<string>(diagram, 0, diagram.Length-1)) {
            for (int i = 0; i < stackNumbers.Length; i++) {
                try {
                    if (!char.IsWhiteSpace(box[multiple])) {
                        if (Stacks.ContainsKey(i + 1)) {
                            List<char> stack = new List<char>();
                            Stacks.TryGetValue(i + 1, out stack);
                            if (stack == null) return;
                            stack.Add(box[multiple]);
                        } else {
                            List<char> stack = new List<char>();
                            stack.Add(box[multiple]);
                            Stacks.Add(i + 1, stack);
                        }
                    }
                } catch(KeyNotFoundException ex) {
                    Console.WriteLine(ex.Message);
                }
                multiple += 4;
            }
            multiple = 1;
        }
        // Reverse all
        foreach (KeyValuePair<int, List<char>> stack in Stacks) {
            stack.Value.Reverse();
        }

    }

    public char GetTopItem(int stackNum) {
        if (!Stacks.ContainsKey(stackNum)) return ';';
        List<char> stack = new List<char>();
        try {
            Stacks.TryGetValue(stackNum, out stack);
            if (stack == null) return ';';
            return stack[stack.Count()-1];
        } catch(KeyNotFoundException ex) {
            Console.WriteLine(ex.Message);
            return ';';
        }
    }

    public bool MoveTopItem(int fromStack, int toStack, int quantity) {
        bool result = false;
        for (int i = 0; i<quantity; i++) {
            result = MoveTopItem(fromStack, toStack);
            if (!result) break;
        }
        return result;
    }

    public bool MoveTopItem(int fromStack, int toStack) {
        if (!Stacks.ContainsKey(fromStack)) return false;
        if (!Stacks.ContainsKey(toStack)) return false;
        List<char> from = new List<char>();
        List<char> to = new List<char>();

        try {
            Stacks.TryGetValue(fromStack, out from);
            if (from == null) return false;
            Stacks.TryGetValue(toStack, out to);
            if (to == null) return false;
            to.Add(from[from.Count-1]);
            from.RemoveAt(from.Count-1);
        } catch(KeyNotFoundException ex) {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
    }


}


