using Discord;
using Discord.Commands;
using TavisBot.Common;
using RunMode = Discord.Commands.RunMode;

namespace TavisBot.Modules;

public class GeneralCommands : ModuleBase<ShardedCommandContext>
{
  public CommandService? CommandService { get; set; }

  [Command("help", RunMode = RunMode.Async)]
  public async Task Help()
  {
    if (!AccessCheck.HasBetaAccess(Context)) return;

    await Context.Message.ReplyAsync($"Hi! I'm your friendly neighborhood bot, " +
      "here to help you with anything BCM related! I sometimes make references to Spider-Man " +
      "because of my deep-rooted trauma of knowing I'll never be able to play those games... " +
      "Anyways, heres what I can do: ");

    await Context.Message.ReplyAsync("Actually, I can't really do anything right now. " +
      "My therapist says I shouldn't say those things but it's true!");

    // TODO: lets not do two separate messages
  }

  [Command("hello", RunMode = RunMode.Async)]
  public async Task Hello()
  {
    if (!AccessCheck.HasBetaAccess(Context)) return;
    await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
  }
}
