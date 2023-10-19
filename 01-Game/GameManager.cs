using System;
using Godot;
using Godot.GameOfLife;

namespace Godot.GameOfLife
{
    public partial class GameManager : Node2D
    {
        [Export]
        public UniTilemap _universe;
        public bool PlayPauseGame()
        {
            _universe.Paused = !_universe.Paused;
            return _universe.Paused;
        }

        public bool IsGamePaused()
        {
            return _universe.Paused;
        }

        public void Step()
        {
            _universe.Step();
        }

        public void Clear()
        {
            _universe.ClearUniverse();

        }

        public void ChangeSpeed(int speed)
        {
            _universe.SimSpeed = speed;
        }
    }
}
