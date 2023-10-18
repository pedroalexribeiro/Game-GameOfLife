using System;
using Godot;

namespace Godot.GameOfLife
{

	public partial class Universe : Node
	{
		[Export]
		private UniTilemap _tilemap;

		[Export]
		public int GridSize = 10;
		public bool Paused = false;
		private int[,] _universe;
		private UniverseState currentState;
		private UniverseState nextState;

		public override void _Ready()
		{
			base._Ready();
			_universe = new int[GridSize, GridSize];
			currentState = new UniverseState(_universe);
			nextState = new UniverseState(_universe);
		}

		public override void _Input(InputEvent @event)
		{
			base._Input(@event);
			if (@event is InputEventMouseButton && Input.IsActionJustPressed("click"))
			{
				Vector2I posMap = _tilemap.LocalToMap(_tilemap.GetLocalMousePosition());
				int state = 0;
				if (_universe[posMap.X, posMap.Y] == 0)
					state = 1;
				_universe[posMap.X, posMap.Y] = state;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			nextState = Step();
			_tilemap.UniState = currentState;
		}

		public UniverseState Step()
		{
			currentState = new UniverseState(_universe);

			for (int i = 0; i < GridSize; i++)
			{
				for (int j = 0; j < GridSize; j++)
				{
					int newTileState = CalculateNewState(currentState, i, j);
					_universe[i, j] = newTileState;
				}
			}
			UniverseState newState = new(_universe);
			return newState;
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
