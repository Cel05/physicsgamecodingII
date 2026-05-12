
public static class RunScoreData
{
    //Scores saved from each levels
    public static int level1Score = 0;
    public static int level2Score = 0;

    //Reset saved scores when new game starts
    public static void ResetScores()
    {
        level1Score = 0;
        level2Score = 0;
    }

    //Returns the combined score from each levels
    public static int TotalScore()
    {
        return level1Score + level2Score;
    }
}