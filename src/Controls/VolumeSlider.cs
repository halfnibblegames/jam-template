using System;
using Godot;

namespace HalfNibbleGame.Controls;

[Tool]
public partial class VolumeSlider : Control {
  private int busIndex;
  private double value;
  private Slider? slider;

  // Called when the node enters the scene tree for the first time.
  public override void _EnterTree() {
    base._EnterTree();

    ChildEnteredTree += onChildAdded;
    ChildExitingTree += onChildRemoved;
  }

  public override void _ExitTree() {
    ChildEnteredTree -= onChildAdded;
    ChildExitingTree -= onChildRemoved;

    base._ExitTree();
  }

  public override void _Ready() {
    base._Ready();
    busIndex = AudioServer.GetBusIndex("Master");
    value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIndex));
  }

  private void onChildAdded(Node child) {
    if (child is not Slider s) return;
    if (slider is not null) GD.PushWarning("Second slider added as child to volume slider. Ignoring.");

    setSlider(s);
  }

  private void setSlider(Slider s) {
    slider = s;
    slider.Value = value;
    slider.ValueChanged += onSliderValueChanged;

    if (Engine.IsEditorHint()) UpdateConfigurationWarnings();
  }

  private void onChildRemoved(Node child) {
    if (child != slider) return;

    unsetSlider();
  }

  private void unsetSlider() {
    if (slider is null) return;
    slider.ValueChanged -= onSliderValueChanged;
    slider = null;

    if (Engine.IsEditorHint()) UpdateConfigurationWarnings();
  }

  private void onSliderValueChanged(double newValue) {
    value = newValue;
    AudioServer.SetBusVolumeDb(busIndex, (float) Mathf.LinearToDb(newValue));
  }

  public override string[] _GetConfigurationWarnings() {
    return slider is null ? new[] { "Volume slider requires a slider as child to work." } : Array.Empty<string>();
  }
}
