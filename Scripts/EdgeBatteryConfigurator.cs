using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.TemplateInstantiation;

namespace pmduda.EdgeBattery.Scripts {
  [Context("Game")]
  public class EdgeBatteryConfigurator : Configurator {

    protected override void Configure() {
      Bind<EdgeBattery>().AsTransient();
      Bind<EdgeBatteryVisualizer>().AsTransient();

      MultiBind<IBlockObjectValidator>().To<EdgeValidator>().AsSingleton();
      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule() {
      var builder = new TemplateModule.Builder();
      builder.AddDecorator<EdgeBatterySpec, EdgeBattery>();
      builder.AddDecorator<EdgeBatteryVisualizerSpec, EdgeBatteryVisualizer>();
      return builder.Build();
    }

  }
}