namespace Godot.GameOfLife
{
    public struct UniverseState
    {
        public UniverseState(int[,] state)
        {
            State = state;
        }

        public int[,] State { get; set; }

        public override readonly string ToString() => $"({State})";
    }
}