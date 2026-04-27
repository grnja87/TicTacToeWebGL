using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGame
{
    public enum CellState { Empty, X, O }
    public enum GameState { Playing, Player1Wins, Player2Wins, Draw }

    public CellState[,] Board { get; private set; } = new CellState[3, 3];

    public int CurrentPlayer { get; private set; } = 1;
    public GameState State { get; private set; } = GameState.Playing;

    public int Player1Moves { get; private set; }
    public int Player2Moves { get; private set; }

    public (int r0, int c0, int r1, int c1) WinLine { get; private set; }

    private static readonly int[][,] WIN_PATTERNS = new int[][,]
    {
        new int[,]{{0,0},{0,1},{0,2}},
        new int[,]{{1,0},{1,1},{1,2}},
        new int[,]{{2,0},{2,1},{2,2}},
        new int[,]{{0,0},{1,0},{2,0}},
        new int[,]{{0,1},{1,1},{2,1}},
        new int[,]{{0,2},{1,2},{2,2}},
        new int[,]{{0,0},{1,1},{2,2}},
        new int[,]{{0,2},{1,1},{2,0}},
    };

    public void Reset() {
        Board = new CellState[3, 3];
        CurrentPlayer = 1;
        State = GameState.Playing;
        Player1Moves = 0;
        Player2Moves = 0;
    }

    public bool PlaceMark(int row, int col) {
        if (State != GameState.Playing) return false;
        if (Board[row, col] != CellState.Empty) return false;

        Board[row, col] = CurrentPlayer == 1 ? CellState.X : CellState.O;

        if (CurrentPlayer == 1) Player1Moves++; else Player2Moves++;

        if (CheckWin(out var winLine)) {
            State = CurrentPlayer == 1 ? GameState.Player1Wins : GameState.Player2Wins;
            WinLine = winLine;
        } else if (IsBoardFull()) {
            State = GameState.Draw;
        } else {
            CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
        }

        return true;
    }

    private bool CheckWin(out (int r0, int c0, int r1, int c1) line) {
        CellState mark = CurrentPlayer == 1 ? CellState.X : CellState.O;
        foreach (var pattern in WIN_PATTERNS) {
            if (Board[pattern[0, 0], pattern[0, 1]] == mark &&
                Board[pattern[1, 0], pattern[1, 1]] == mark &&
                Board[pattern[2, 0], pattern[2, 1]] == mark) {
                line = (pattern[0, 0], pattern[0, 1], pattern[2, 0], pattern[2, 1]);
                return true;
            }
        }
        line = default;
        return false;
    }

    private bool IsBoardFull() {
        foreach (var cell in Board)
            if (cell == CellState.Empty) return false;
        return true;
    }
}
