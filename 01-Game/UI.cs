using System;
using Godot;
using Godot.GameOfLife;

public partial class UI : CanvasLayer
{
    [Export]
    private Button _playButton;
    [Export]
    private Button _stepButton;
    [Export]
    private Button _clearButton;
    [Export]
    private GameManager _gameManager;

    public override void _Ready()
    {
        base._Ready();
        _playButton.ButtonDown += OnPlayButtonPressed;
        _stepButton.ButtonDown += OnStepButtonPressed;
        _clearButton.ButtonDown += OnClearButtonPressed;
    }

    private void OnClearButtonPressed()
    {
        _gameManager.Clear();
    }

    private void OnStepButtonPressed()
    {
        _gameManager.Step();
    }

    private void OnPlayButtonPressed()
    {
        _gameManager.PlayPauseGame();
    }
}
