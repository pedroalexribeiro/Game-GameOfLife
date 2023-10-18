namespace Godot.GameOfLife{
    public readonly struct UniverseState
    {
        public UniverseState(int[,] state)
        {
            State = state;
        }

        public int[,] State { get; }

        public override readonly string ToString() => $"({State})";
}
}