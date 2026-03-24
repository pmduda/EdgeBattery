using System;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.TickSystem;
using UnityEngine;

namespace pmduda.EdgeBattery.Scripts {
  internal class EdgeBatteryVisualizer : TickableComponent,
                                         IAwakableComponent,
                                         IUpdatableComponent {

    private static readonly float MinimumChangeRate = 0.001f;
    private static readonly float MaxOccupancyRangeDifference = 0.25f;

    private readonly ITickService _tickService;
    private EdgeBattery _edgeBattery;
    private Transform _positionTransform;
    private Transform _scaleTransform;
    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    private float _previousOccupancyRange;
    private float _occupancyChangeRate;

    public EdgeBatteryVisualizer(ITickService tickService) {
      _tickService = tickService;
    }

    public void Awake() {
      _edgeBattery = GetComponent<EdgeBattery>();
      var spec = GetComponent<EdgeBatteryVisualizerSpec>();
      _positionTransform = GameObject.FindChildTransform(spec.PositionTransformName);
      _scaleTransform = GameObject.FindChildTransform(spec.ScaleTransformName);
      _originalPosition = _positionTransform.localPosition;
      _originalScale = _scaleTransform.localScale;
    }

    public void Update() {
      UpdateTransforms();
    }

    public override void StartTickable() {
      UpdatePositionAndScale(_edgeBattery.OccupancyRange);
    }

    public override void Tick() {
      UpdateOccupancyChangeRate();
    }

    private void UpdateTransforms() {
      var currentOccupancyRange = _originalPosition.y - _positionTransform.localPosition.y;
      var targetOccupancyRange = _edgeBattery.OccupancyRange;
      if (Math.Abs(currentOccupancyRange - targetOccupancyRange) > MaxOccupancyRangeDifference) {
        UpdatePositionAndScale(targetOccupancyRange);
      } else {
        Interpolate(currentOccupancyRange, targetOccupancyRange);
      }
    }

    private void UpdateOccupancyChangeRate() {
      var occupancyChangeRate =
          Math.Abs((_edgeBattery.OccupancyRange - _previousOccupancyRange)
                   / _tickService.TickIntervalInSeconds);
      _occupancyChangeRate = Math.Max(MinimumChangeRate, occupancyChangeRate);
      _previousOccupancyRange = _edgeBattery.OccupancyRange;
    }

    private void Interpolate(float currentOccupancyRange, float targetOccupancyRange) {
      var newOccupancyRange = Mathf.MoveTowards(currentOccupancyRange, targetOccupancyRange,
                                                Time.deltaTime * _occupancyChangeRate);
      UpdatePositionAndScale(newOccupancyRange);
    }

    private void UpdatePositionAndScale(float newOccupancyRange) {
      _positionTransform.localPosition = _originalPosition - new Vector3(0, newOccupancyRange, 0);
      _scaleTransform.localScale = _originalScale + new Vector3(0, newOccupancyRange, 0);
    }

  }
}