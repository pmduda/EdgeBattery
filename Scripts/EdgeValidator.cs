using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.Localization;
using Timberborn.TerrainSystem;

namespace pmduda.EdgeBattery.Scripts {
  internal class EdgeValidator : IBlockObjectValidator {

    private static readonly string MustBeOnEdgeLocKey = "Buildings.MustBeOnEdge";
    private readonly ILoc _loc;
    private readonly ITerrainService _terrainService;

    public EdgeValidator(ILoc loc, ITerrainService terrainService) {
      _loc = loc;
      _terrainService = terrainService;
    }

    public bool IsValid(BlockObject blockObject, out string errorMessage) {
      if (blockObject.GetComponent<EdgeBattery>()
          && _terrainService.Contains(blockObject.Coordinates.XY())) {
        errorMessage = _loc.T(MustBeOnEdgeLocKey);
        return false;
      }
      errorMessage = default;
      return true;
    }

  }
}