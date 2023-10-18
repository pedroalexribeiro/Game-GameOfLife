using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
    [Export]
    public float speed = 400;
    public override void _Process(double delta)
    {
        base._Process(delta);
        Vector2 moveVector = new();
        if(Input.IsActionPressed("move_up")){
            moveVector.Y -= 1;
        }
        if(Input.IsActionPressed("move_down")){
            moveVector.Y += 1;
        }
        if(Input.IsActionPressed("move_right")){
            moveVector.X += 1;
        }
        if(Input.IsActionPressed("move_left")){
            moveVector.X -= 1;
        }

        if(Input.IsActionJustReleased("scroll_up"))
            Zoom += new Vector2(0.1f, 0.1f);
        if(Input.IsActionJustReleased("scroll_down"))
            Zoom -= new Vector2(0.1f, 0.1f);

        Position += moveVector * speed * (float)delta;
    }
}
