using Discord.Commands;
using Discord.WebSocket;
using static TavisBot.Common.Constant;

namespace TavisBot.Common;

public static class AccessCheck
{
  public static bool HasBetaAccess(ShardedCommandContext context)
  {
    var betaRoles = new List<Roles>() { Roles.Admin, Roles.Mod, Roles.Sponsor, Roles.Enthusiast };
    var user = context.User as SocketGuildUser;
    if (user == null) return false;
    return user.Roles.Any(role => betaRoles.Any(betaRole => role.Name == betaRole.GetDescription()));
  }
}

