using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleEvent[] puzzles;

    public UnityEvent OnAllSolved;

    private bool[] solvedStates;

    private bool allSolved = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (puzzles == null || puzzles.Length == 0)
        {
            Debug.Log("puzzle manager has no puzzles assigned");
            return;
        }
        
        solvedStates = new bool[puzzles.Length];
        for (int i = 0; i < puzzles.Length; i++)
        {
            int index = 1;
            puzzles[i].OnCompleted.AddListener(() => OnPuzzleSolved(index));
            puzzles[i].OnReset.AddListener(() => OnPuzzleReset(index));
        }
    }

    void OnPuzzleSolved(int index)
    {
        solvedStates[index] = true;
        Debug.Log($"Puzzle {index} solved. Checking all puzzles....");
        CheckAllSolved();
    }

    void OnPuzzleReset(int index)
    {
        solvedStates[index] = false;
        allSolved = false;
        Debug.Log($"Puzzle {index} reset");
    }

    void CheckAllSolved()
    {
        foreach (bool state in solvedStates)
        {
            if (!state) return;
        }

        if (!allSolved)
        {
            allSolved = true;
            Debug.Log("all puzzles solved");
            OnAllSolved?.Invoke();
        }
    }
}
