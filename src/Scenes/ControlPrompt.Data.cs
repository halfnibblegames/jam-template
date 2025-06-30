using System;

namespace HalfNibbleGame.Scenes;

public partial class ControlPrompt {
  private static ControlData createData(ControlInput input) => input switch {
    ControlInput.ControllerButtonA => new ControlData(47, 42),
    ControlInput.ControllerButtonB => new ControlData(48, 43),
    ControlInput.ControllerButtonX => new ControlData(49, 44),
    ControlInput.ControllerButtonY => new ControlData(50, 45),
    _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
  };

  private readonly record struct ControlData(int FrameNo, int PressedFrameNo);
}
