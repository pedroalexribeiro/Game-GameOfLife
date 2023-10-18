using System;
using Godot;
using Godot.GameOfLife;

namespace Godot.GameOfLife
{
    public partial class GameManager : Node2D
    {
        [Export]
        public Universe _universe;
        public void PlayPauseGame()
        {
            _universe.Paused = !_universe.Paused;
        }

        public void Step()
        {
            _universe.Step();
        }

        public void Clear()
        {
            // Reset Universe
        }
    }
}
