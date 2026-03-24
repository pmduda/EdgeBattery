using Timberborn.BlueprintSystem;

namespace pmduda.EdgeBattery.Scripts {
  internal record EdgeBatteryVisualizerSpec : ComponentSpec {

    [Serialize]
    public string PositionTransformName { get; init; }

    [Serialize]
    public string ScaleTransformName { get; init; }

  }
}