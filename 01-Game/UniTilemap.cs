using System;
using Godot;

namespace Godot.GameOfLife
{
    public partial class UniTilemap : TileMap
    {
        public UniverseState UniState;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (UniState.State == null)
                return;
            int[,] stateToDraw = UniState.State;
            for (int i = 0; i < stateToDraw.GetLength(0); i++)
            {
                for (int j = 0; j < stateToDraw.GetLength(1); j++)
                {
                    Vector2I cellPos = new(i, j);
                    Vector2I atlasCord = new(0, 0);

                    // If alive chooses atlas 1 which is WhiteBox
                    // If dead chooses atrlas 0 which is BlackBox
                    SetCell(0, cellPos, stateToDraw[i, j], atlasCord, 0);
                }
            }
        }
    }
}
