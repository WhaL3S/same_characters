class Program
    {
        // Method to compare characters of the word with the current character to avoid repetitions
        static bool Comparison(string word, int i, char[] repeated, int repeatedCounter)
        {
            for(int k = 0; k < repeatedCounter; k++)
                if (word[i] == repeated[k])
                    return false;
            return true;
        }

        // Method to insert count of same characters and the total number of characters of each word
        static void Insert(string myLine, char[] delimiters, StringBuilder mystringBuilder, ref int parenthesisCounter, ref bool checker)
        {
            // Counter for number of parenthesis
            parenthesisCounter = 0;

            // Separating line to words
            string[] words = myLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            myLine = " " + myLine + " ";
            
            // Index of the word in line
            int ind = 0;

            // Position for StringBuilder
            int init = 1;

            // Checker for case when there are only delimiters in the text
            checker = false;

            foreach (string word in words)
            {
                // Array to store repeated characters
                char[] repeated = new char[word.Length];
                int repeatedCounter = 0;

                // Array to save location of already counted characters
                int[] index = new int[word.Length + 1];

                // Array to count number of repetetions of each character
                int[] counter = new int[word.Length + 1];
                int counterCounter = 0;

                int counterBasic;
                int j = -1;

                // Considering uppercase and lowercase as the same character
                string wordBuffer = word.ToLower();

                // Filling all necessary arrays  
                for (int i = 0; i < word.Length; i++)
                {
                    if (Comparison(wordBuffer, i, repeated, repeatedCounter) == true)
                    {
                        counterBasic = 1;
                        for (j = i + 1; j < word.Length; j++)
                        {
                            char first = wordBuffer[i];
                            char second = wordBuffer[j];

                            if (wordBuffer[i] == wordBuffer[j])
                            {
                                counterBasic++;
                                if (counterBasic > 1)
                                    repeated[repeatedCounter++] = wordBuffer[j];
                            }
                        }
                        counter[counterCounter] = counterBasic;
                        index[counterCounter++] = i;
                    }
                }

                // Finding location of the word
                ind = myLine.IndexOf(word, ind + 1);
                
                // Addition of lines and parenthesis with data into the StringBuilder
                if (word.Length == 1)
                {
                    mystringBuilder.Append(myLine.Substring(init, ind + word.Length + 1 - init));
                    mystringBuilder.Append("(number of characters: " + word.Length + "; ");
                }
                else
                {
                    mystringBuilder.Append(myLine.Substring(init, ind + word.Length - init));
                    mystringBuilder.Append(" (number of characters: " + word.Length + "; ");
                }
                for (int i = 0; i < counterCounter; i++)
                {
                    if (i == counterCounter - 1)
                        mystringBuilder.Append(wordBuffer[index[i]] + " is " + counter[i]);
                    else
                        mystringBuilder.Append(wordBuffer[index[i]] + " is " + counter[i] + "; ");
                }
                mystringBuilder.Append(")");
                init = ind + word.Length;
                
                // Counting number of parenthesises
                parenthesisCounter++;

                checker = true;
            }
        }

        // Method to read given txt file, to fill results and analysis files
        static void Process(string fd, string fr, string fa, char[] delimeters)
        {
            using(StreamReader reader = new StreamReader(fd))
            {
                using (StreamWriter writer = new StreamWriter(fr, false))
                {
                    using (StreamWriter analyzer = new StreamWriter(fa, false))
                    {
                        // Elements to draw table
                        string top = new string('-', 39);
                        analyzer.WriteLine(top);
                        analyzer.WriteLine("| Line number | Number of Parenthesis |");
                        analyzer.WriteLine(top);

                        string line;
                        int lineCounter = 0;
                        bool checker = true;
                        
                        // Read and fill txt files
                        while ((line = reader.ReadLine()) != null)
                        {
                            int parenthesisCounter = 0;
                            if (line.Length > 0)
                            {
                                StringBuilder appendedStringBuilder = new StringBuilder();
                                Insert(line, delimeters, appendedStringBuilder, ref parenthesisCounter, ref checker);
                                if (checker == true)
                                    writer.WriteLine(appendedStringBuilder);
                                else
                                    writer.WriteLine(line);
                            }
                            else 
                                writer.WriteLine(line);
                            ++lineCounter;
                            analyzer.WriteLine("| {0, 11} | {1, 21} |", lineCounter, parenthesisCounter);
                        }
                        analyzer.WriteLine(top);
                    }
                }
            }
        }

        // Constant txt files
        const string CFd = "L4_7T.txt";
        const string CFa = "Analysis.txt";
        const string CFr = "Results.txt";

        // Main body where code runs
        static void Main(string[] args)
        {
            // Array of delimiters
            char[] delimiters = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t', '{', '}', '[', ']'};
            
            // Running the process
            Process(CFd, CFr, CFa, delimiters);
        }
    }
