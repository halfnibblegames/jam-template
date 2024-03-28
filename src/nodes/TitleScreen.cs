using Godot;
using JetBrains.Annotations;

[UsedImplicitly]
public class TitleScreen : Control
{
    private const float animationDelay = 1.2f;

    private float elapsedTime;
    private bool animationStarted;

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        if (animationStarted)
            return;

        elapsedTime += delta;
        if (animationDelay > elapsedTime)
            return;

        animationStarted = true;
        GetNode<AudioStreamPlayer>("BiteSound").Play();
        GetNode<AnimatedSprite>("Strawberry").Play("Bite");
    }

    [UsedImplicitly]
    public void OnBiteAnimationFinished()
    {
        var tween = new Tween();
        AddChild(tween);
        const float durationSeconds = 0.2f;
        tween.InterpolateProperty(
            GetNode("Copyright"),
            "margin_top",
            40,
            -40,
            durationSeconds
        );
        tween.Start();
    }
}
