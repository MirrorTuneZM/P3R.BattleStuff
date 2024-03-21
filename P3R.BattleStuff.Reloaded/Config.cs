using P3R.BattleStuff.Reloaded.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;

namespace P3R.BattleStuff.Reloaded.Configuration;

public class Config : Configurable<Config>
{
    public const int TartarusHpSp_DotTimeSeconds_DEFAULT = 30;
    public const int TartarusHpSp_MaxHpDotRatio_DEFAULT = 2;
    public const int TartarusHpSp_MaxSpDotRatio_DEFAULT = 2;

    public const double BtlNormal_ExpMuti_DEFAULT = 0.75;
    public const double BtlNormal_EnemyDamageMuti_DEFAULT = 1;

    public const double BtlAdvantage_ExpMuti_DEFAULT = 1.20;
    public const double BtlAdvantage_EnemyDamageMuti_DEFAULT = 1;

    public const double BtlDisadvantage_ExpMuti_DEFAULT = 1;
    public const double BtlDisadvantage_EnemyDamageMuti_DEFAULT = 1.15;

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

    [Category("Battle Module: EXP")]
    [DisplayName("Normal Multiplier")]
    [DefaultValue(BtlNormal_ExpMuti_DEFAULT)]
    public double BtlNormalExpMulti { get; set; } = BtlNormal_ExpMuti_DEFAULT;

    [Category("Battle Module: Enemy Damage")]
    [DisplayName("Normal Multiplier")]
    [DefaultValue(BtlNormal_EnemyDamageMuti_DEFAULT)]
    public double BtlNormalDamageMulti { get; set; } = BtlNormal_EnemyDamageMuti_DEFAULT;

    [Category("Battle Module: EXP")]
    [DisplayName("Advantage Multiplier")]
    [DefaultValue(BtlAdvantage_ExpMuti_DEFAULT)]
    public double BtlAdvantageExpMulti { get; set; } = BtlAdvantage_ExpMuti_DEFAULT;

    [Category("Battle Module: Enemy Damage")]
    [DisplayName("Advantage Multiplier")]
    [DefaultValue(BtlAdvantage_EnemyDamageMuti_DEFAULT)]
    public double BtlAdvantageDamageMulti { get; set; } = BtlAdvantage_EnemyDamageMuti_DEFAULT;

    [Category("Battle Module: EXP")]
    [DisplayName("Disadavantage Multiplier")]
    [DefaultValue(BtlDisadvantage_ExpMuti_DEFAULT)]
    public double BtlDisadvantageExpMulti { get; set; } = BtlDisadvantage_ExpMuti_DEFAULT;

    [Category("Battle Module: Enemy Damage")]
    [DisplayName("Disadavantage Multiplier")]
    [DefaultValue(BtlDisadvantage_EnemyDamageMuti_DEFAULT)]
    public double BtlDisadvantageDamageMulti { get; set; } = BtlDisadvantage_EnemyDamageMuti_DEFAULT;
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
    // 
}