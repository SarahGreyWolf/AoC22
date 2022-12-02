class Day2 {

    const char OPP_ROCK = 'A';
    const char OPP_PAPER = 'B';
    const char OPP_SCISSOR = 'C';
    const char P_ROCK = 'X';
    const char P_PAPER = 'Y';
    const char P_SCISSOR = 'Z';

    const int SCORE_ROCK = 1;
    const int SCORE_PAPER = 2;
    const int SCORE_SCISSOR = 3;
    const int LOST = 0;
    const int DRAW = 3;
    const int WIN = 6;

    public Day2() {

    }

    static void Main(string[] args) {
        string path = args[0];
        string[] input = File.ReadAllLines(path);


        int finalScore = 0;
        foreach (var line in input) {
            string[] chars = line.Split(' ');
            int score = Day2.DetermineScore(chars[0][0], chars[1][0]);
            Console.WriteLine("Players this round would be " + score);
            finalScore += score;
        }

        Console.WriteLine("Players final score would be " + finalScore);
        
        
    }

    public static int DetermineScore(char opponent, char player) {
        if (opponent == OPP_ROCK && player == P_ROCK) {
            return SCORE_ROCK + DRAW;
        }
        if (opponent == OPP_PAPER && player == P_PAPER) {
            return SCORE_PAPER + DRAW;
        }
        if (opponent == OPP_SCISSOR && player == P_SCISSOR) {
            return SCORE_SCISSOR + DRAW;
        }
        if (opponent == OPP_ROCK && player == P_SCISSOR) {
            return SCORE_SCISSOR;
        }
        if (opponent == OPP_ROCK && player == P_PAPER) {
            return SCORE_PAPER + WIN;
        }
        if (opponent == OPP_SCISSOR && player == P_PAPER) {
            return SCORE_PAPER;
        }
        if (opponent == OPP_SCISSOR && player == P_ROCK) {
            return SCORE_ROCK + WIN;
        }
        if (opponent == OPP_PAPER && player == P_ROCK) {
            return SCORE_ROCK;
        }
        if (opponent == OPP_PAPER && player == P_SCISSOR) {
            return SCORE_SCISSOR + WIN;
        }
        return 0;
    }
}
