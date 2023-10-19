using System;
using Godot;

namespace Godot.GameOfLife
{
    public partial class UniTilemap : TileMap
    {
        [Export]
        public int GridSize = 20;

        [Export]
        public int BoxSize = 8;

        [Export]
        public Camera2D _camera;
        public bool Paused = true;
        public int SimSpeed = 2;
        private UniverseState currentState;
        private UniverseState nextState;

        private double _timePassed = 0;

        public override void _Draw()
        {
            base._Draw();

            Vector2 size = GetViewportRect().Size; //* _camera.Zoom / 2;
            Vector2 cam = _camera.Position;

            for (int i = ((int)(cam.X - size.X) / BoxSize) - 1; i < ((int)(size.X + cam.X) / BoxSize) + 1; i++)
                DrawLine(new Vector2(i * BoxSize, cam.Y + size.Y + 100), new Vector2(i * BoxSize, cam.Y - size.Y - 100), Colors.WebGray);

            for (int i = ((int)(cam.Y - size.Y) / BoxSize) - 1; i < ((int)(size.Y + cam.Y) / BoxSize) + 1; i++)
            {
                DrawLine(new Vector2(cam.X + size.X + 100, i * BoxSize), new Vector2(cam.X - size.X - 100, i * BoxSize), Colors.WebGray);
            }
        }


        public override void _Ready()
        {
            base._Ready();
            int[,] _currentUniverse = new int[GridSize, GridSize];
            int[,] _nextUniverse = new int[GridSize, GridSize];
            currentState = new UniverseState(_currentUniverse);
            nextState = new UniverseState(_nextUniverse);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            if (@event is InputEventMouseButton && Input.IsActionJustPressed("click"))
            {
                Vector2I posMap = LocalToMap(GetLocalMousePosition());
                int[,] uniState = currentState.State;
                int state = 0;
                if (uniState[posMap.X, posMap.Y] == 0)
                    state = 1;
                uniState[posMap.X, posMap.Y] = state;

                // If alive chooses atlas 1 which is WhiteBox
                // If dead chooses atrlas 0 which is BlackBox
                Vector2I atlasCord = new(0, 0);
                SetCell(0, posMap, state, atlasCord, 0);
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            QueueRedraw();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            _timePassed += delta;
            if (!Paused && (float)_timePassed >= 1 / (float)SimSpeed)
            {
                Step();
                _timePassed = 0;
            }
        }

        public void Step()
        {
            nextState.State = (int[,])currentState.State.Clone();
            int[,] nextUniverse = nextState.State;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    int newTileState = CalculateNewState(currentState, i, j);
                    nextUniverse[i, j] = newTileState;
                    Vector2I cellPos = new(i, j);
                    Vector2I atlasCord = new(0, 0);

                    // If alive chooses atlas 1 which is WhiteBox
                    // If dead chooses atrlas 0 which is BlackBox
                    SetCell(0, cellPos, newTileState, atlasCord, 0);
                }
            }
            currentState.State = (int[,])nextState.State.Clone();
        }

        public void ClearUniverse()
        {
            int[,] currentUniverse = new int[GridSize, GridSize];
            currentState = new UniverseState(currentUniverse);
            nextState = new UniverseState(currentUniverse);

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Vector2I cellPos = new(i, j);
                    Vector2I atlasCord = new(0, 0);

                    // If alive chooses atlas 1 which is WhiteBox
                    // If dead chooses atrlas 0 which is BlackBox
                    SetCell(0, cellPos, 0, atlasCord, 0);
                }
            }
        }

        private int CalculateNewState(UniverseState universeState, int i, int j)
        {
            int[,] universe = universeState.State;
            int neighbors = CalculateNumberOfNeighbors(universeState, i, j);
            if (neighbors < 2)
                return 0;
            if (neighbors > 3)
                return 0;

            if (universe[i, j] == 1)
                return 1;
            if (universe[i, j] == 0 && neighbors == 3)
                return 1;
            return 0;
        }

        private int CalculateNumberOfNeighbors(UniverseState universeState, int i, int j)
        {
            int[,] universe = universeState.State;
            int neighbors = 0;
            // Upper left
            if (i > 0 && j > 0 && universe[i - 1, j - 1] == 1)
                neighbors++;
            // Upper middle
            if (i > 0 && universe[i - 1, j] == 1)
                neighbors++;
            // Upper right
            if (i > 0 && j < GridSize - 1 && universe[i - 1, j + 1] == 1)
                neighbors++;
            // Middle left
            if (j > 0 && universe[i, j - 1] == 1)
                neighbors++;
            // Middle right
            if (j < GridSize - 1 && universe[i, j + 1] == 1)
                neighbors++;
            // Lower left
            if (i < GridSize - 1 && j > 0 && universe[i + 1, j - 1] == 1)
                neighbors++;
            // Lower middle
            if (i < GridSize - 1 && universe[i + 1, j] == 1)
                neighbors++;
            // Lower right
            if (i < GridSize - 1 && j < GridSize - 1 && universe[i + 1, j + 1] == 1)
                neighbors++;
            return neighbors;
        }
    }
}
