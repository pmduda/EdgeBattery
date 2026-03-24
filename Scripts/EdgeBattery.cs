using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.MechanicalSystem;
using UnityEngine;

namespace pmduda.EdgeBattery.Scripts {
  public class EdgeBattery : BaseComponent,
                             IAwakableComponent,
                             IFinishedStateListener,
                             IBattery {

    private static readonly float MaxOccupancyRange = 100f;
    internal float OccupancyRange { get; private set; }
    private MechanicalNode _mechanicalNode;
    private EdgeBatterySpec _edgeBatterySpec;

    public void Awake() {
      _mechanicalNode = GetComponent<MechanicalNode>();
      _edgeBatterySpec = GetComponent<EdgeBatterySpec>();
    }

    public void OnEnterFinishedState() {
      UpdateNode();
    }

    public void OnExitFinishedState() {
    }

    public void ModifyCharge(float chargeDelta) {
      var lengthDelta = -chargeDelta / CapacityPerTile;
      OccupancyRange = Mathf.Clamp(OccupancyRange + lengthDelta, 0, MaxOccupancyRange);

      UpdateNode();
    }

    private int CapacityPerTile => _edgeBatterySpec.CapacityPerTile;

    private void UpdateNode() {
      _mechanicalNode.SetNominalBatteryCharge(
          Mathf.CeilToInt(CapacityPerTile * (MaxOccupancyRange - OccupancyRange)));
      _mechanicalNode.SetNominalBatteryCapacity(
          Mathf.CeilToInt(MaxOccupancyRange * CapacityPerTile));
    }

  }
}