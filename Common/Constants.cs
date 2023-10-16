using System.ComponentModel;

namespace TavisBot.Common;

public static class Constant
{
  public const string TavisCmdShorthand = ".t ";
  public const string TavisCmd = ".tavis ";
  public enum Roles
  {
    [Description("Admin")]
    Admin,
    [Description("Mod")]
    Mod,
    [Description("Sponsor")]
    Sponsor,
    [Description("Enthusiast")]
    Enthusiast,
    // TODO: these aren't all the roles
  }
}
