using P3R.BattleStuff.Reloaded.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;

namespace P3R.BattleStuff.Reloaded.Configuration;

public class Config : Configurable<Config>
{
    public const int TartarusHpSp_DotTimeSeconds_DEFAULT = 30;
    public const int TartarusHpSp_MaxHpDotRatio_DEFAULT = 2;
    public const int TartarusHpSp_MaxSpDotRatio_DEFAULT = 2;

    [DisplayName("Log Level")]
    [DefaultValue(LogLevel.Information)]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    [Category("Tartarus HP/SP Module")]
    [DisplayName("HP/SP DOT Seconds")]
    [DefaultValue(TartarusHpSp_DotTimeSeconds_DEFAULT)]
    public int TartarusHpSp_DotTimeSeconds { get; set; } = TartarusHpSp_DotTimeSeconds_DEFAULT;

    [Category("Tartarus HP/SP Module")]
    [DisplayName("% Max HP Damage to HP")]
    [DefaultValue(TartarusHpSp_MaxHpDotRatio_DEFAULT)]
    [SliderControlParams(
            minimum: 0.0,
            maximum: 100.0,
            smallChange: 1,
            largeChange: 10,
            tickFrequency: 1,
            isSnapToTickEnabled: true,
            tickPlacement: SliderControlTickPlacement.None,
            showTextField: true)]
    public int TartarusHpSp_MaxHpDotRatio { get; set; } = TartarusHpSp_MaxHpDotRatio_DEFAULT;

    [Category("Tartarus HP/SP Module")]
    [DisplayName("% Max SP Damage to SP")]
    [DefaultValue(TartarusHpSp_MaxSpDotRatio_DEFAULT)]
    [SliderControlParams(
            minimum: 0.0,
            maximum: 100.0,
            smallChange: 1,
            largeChange: 10,
            tickFrequency: 1,
            isSnapToTickEnabled: true,
            tickPlacement: SliderControlTickPlacement.None,
            showTextField: true)]
    public int TartarusHpSp_MaxSpDotRatio { get; set; } = TartarusHpSp_MaxSpDotRatio_DEFAULT;
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
    // 
}