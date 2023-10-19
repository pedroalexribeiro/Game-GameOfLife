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

    [Export]
    private HSlider _speedSlider;

    public override void _Ready()
    {
        base._Ready();
        _playButton.ButtonDown += OnPlayButtonPressed;
        _stepButton.ButtonDown += OnStepButtonPressed;
        _clearButton.ButtonDown += OnClearButtonPressed;
        _speedSlider.ValueChanged += onSpeedSliderChange;

        bool isGamePaused = _gameManager.IsGamePaused();
        if (isGamePaused)
            _playButton.Text = "Play";
        else
            _playButton.Text = "Pause";
    }

    private void onSpeedSliderChange(double value)
    {
        _gameManager.ChangeSpeed((int)value);
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
        bool isGamePaused = _gameManager.PlayPauseGame();
        if (isGamePaused)
            _playButton.Text = "Play";
        else
            _playButton.Text = "Pause";
    }
}
