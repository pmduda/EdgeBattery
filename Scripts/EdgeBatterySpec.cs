using Timberborn.BlueprintSystem;

namespace pmduda.EdgeBattery.Scripts {
  internal record EdgeBatterySpec : ComponentSpec {

    [Serialize]
    public int CapacityPerTile { get; init; }

  }
}