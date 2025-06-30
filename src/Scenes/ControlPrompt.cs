using Godot;

namespace HalfNibbleGame.Scenes;

[Tool]
public partial class ControlPrompt : Control {
  private const int tileSize = 16;
  private const double animationSpeed = 3;

  private Texture2D? imageTexture;
  private int iconsPerRow;

  private AnimatedSprite2D animatedSprite = null!;
  private SpriteFrames spriteFrames = new();
  private ControlInput shownInput;

  [Export]
  public ControlInput ShownInput {
    get => shownInput;
    set {
      if (shownInput == value) return;
      shownInput = value;
      updateAnimation();
    }
  }

  public override void _Ready() {
    animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
    animatedSprite.SpriteFrames = spriteFrames;
    updateAnimation();
  }

  private void updateAnimation() {
    var animationName = animationKey(shownInput);
    if (!spriteFrames.HasAnimation(animationName)) {
      loadAnimation(shownInput);
    }
    animatedSprite.Play(animationName);
  }

  private void loadAnimation(ControlInput input) {
    imageTexture ??= loadTexture(out iconsPerRow);

    var key = animationKey(input);
    var data = createData(input);

    var normalFrame = atlas(data.FrameNo);
    var pressedFrame = atlas(data.PressedFrameNo);

    spriteFrames.AddAnimation(key);
    spriteFrames.SetAnimationSpeed(key, animationSpeed);
    spriteFrames.AddFrame(key, normalFrame, 2);
    spriteFrames.AddFrame(key, pressedFrame);
  }

  private static Texture2D loadTexture(out int iconsPerRow) {
    var texture = GD.Load<Texture2D>("uid://b8builnlxpm41");
    iconsPerRow = texture.GetWidth() / tileSize;
    GD.Print("Loaded texture");
    return texture;
  }

  private AtlasTexture atlas(int frameNo) => new() { Atlas = imageTexture!, Region = regionForFrame(frameNo) };

  private Rect2 regionForFrame(int frameNo) {
    var row = frameNo / iconsPerRow;
    var col = frameNo % iconsPerRow;
    return new Rect2(col * tileSize, row * tileSize, tileSize, tileSize);
  }

  private static string animationKey(ControlInput input) => input.ToString();
}
