using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TicTacToe : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Button[] cellButtons=new Button[9];
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restart;

    private int currentPlayer;
    private int[] cellStates;
    private bool isGameOver;

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {        
        cellStates = new int[9];
        isGameOver = false;

        for (int i = 0; i < cellButtons.Length; i++)
        {
            cellStates[i]=0;
            cellButtons[i].GetComponent<Image>().color=Color.gray;
            cellButtons[i].interactable = true;
            int ii = i;
            cellButtons[i].onClick.AddListener(()=>CellButtonOnClick(ii));
        }

        gameOverPanel.SetActive(false);
        infoText.text = ((currentPlayer==1)?"Player":"AI") + "'s turn";

        restart.interactable = true;
        restart.onClick.AddListener(()=>RestartButtonOnClick());
        currentPlayer = Random.Range(1,3);
        Debug.Log(currentPlayer);
        if (currentPlayer == 2)
        {
            StartCoroutine(AITurn());
        }
    }

    public void CellButtonOnClick(int cellIndex)
    {        
        
        if (isGameOver || cellStates[cellIndex] != 0)
        {
            return;
        }

        cellStates[cellIndex] = currentPlayer;
        cellButtons[cellIndex].GetComponent<Image>().color = (currentPlayer == 1) ? Color.yellow : Color.red ;
        cellButtons[cellIndex].interactable = false;

        if (CheckWin())
        {
            isGameOver = true;
            gameOverText.text = ((currentPlayer==1)?"Player":"AI") + " wins!";
            gameOverPanel.SetActive(true);
        }
        else if (CheckDraw())
        {
            isGameOver = true;
            gameOverText.text = "Draw!";
            gameOverPanel.SetActive(true);
        }
        else
        {
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
            infoText.text = ((currentPlayer==1)?"Player":"AI") + "'s turn";

            if (currentPlayer == 2)
            {
                StartCoroutine(AITurn());
            }
        }
    }

    private IEnumerator AITurn()
    {        
        yield return new WaitForSeconds(1f);
        int bestMove = GetBestMove();
        CellButtonOnClick(bestMove);
        
    }

    private int GetBestMove()
    {
        int[] scores = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (cellStates[i] == 0)
            {
                cellStates[i] = 2;
                scores[i] = Minimax(cellStates, 0, false);
                cellStates[i] = 0;
            }
        }

        int bestScore = int.MinValue;
        int bestMove = -1;
        for (int i = 0; i < 9; i++)
        {
            if (scores[i] > bestScore&&cellStates[i]==0)
            {
                bestScore = scores[i];
                bestMove = i;
            }
        }

        return bestMove;
    }

    private int Minimax(int[] state, int depth, bool isMaximizingPlayer)
    {
        if (CheckWin(state, 2))
        {
            return 10 - depth;
        }
        else if (CheckWin(state, 1))
        {
            return depth - 10;
        }
        else if (CheckDraw(state))
        {
            return 0;
        }

        if (isMaximizingPlayer)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (state[i] == 0)
                {
                    state[i] = 2;
                    int score = Minimax(state, depth + 1, false);
                    state[i] = 0;
                    bestScore = Mathf.Max(bestScore, score);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (state[i] == 0)
                {
                    state[i] = 1;
                    int score = Minimax(state, depth + 1, true);
                    state[i] = 0;
                    bestScore = Mathf.Min(bestScore, score);
                }
            }
            return bestScore;
        }
    }

    private bool CheckWin()
    {
        return CheckWin(cellStates, currentPlayer);
    }

    private bool CheckWin(int[] state, int player)
    {
        return (state[0] == player && state[1] == player && state[2] == player) ||
               (state[3] == player && state[4] == player && state[5] == player) ||
               (state[6] == player && state[7] == player && state[8] == player) ||
               (state[0] == player && state[3] == player && state[6] == player) ||
               (state[1] == player && state[4] == player && state[7] == player) ||
               (state[2] == player && state[5] == player && state[8] == player) ||
               (state[0] == player && state[4] == player && state[8] == player) ||
               (state[2] == player && state[4] == player && state[6] == player);
    }

    private bool CheckDraw()
    {
        return CheckDraw(cellStates);
    }

    private bool CheckDraw(int[] state)
    {
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public void RestartButtonOnClick()
    {        
        
        NewGame();
    }
}

