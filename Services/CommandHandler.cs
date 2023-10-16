using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TavisBot.Common;
using TavisBot.Init;

namespace TavisBot.Services;

public class CommandHandler : ICommandHandler
{
  private readonly DiscordShardedClient _client;
  private readonly CommandService _commands;

  public CommandHandler(
      DiscordShardedClient client,
      CommandService commands)
  {
    _client = client;
    _commands = commands;
  }

  public async Task InitializeAsync()
  {
    // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
    await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Bootstrapper.ServiceProvider);

    // Subscribe a handler to see if a message invokes a command.
    _client.MessageReceived += HandleCommandAsync;

    _commands.CommandExecuted += async (optional, context, result) =>
    {
      if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
      {
        await context.Channel.SendMessageAsync($"error: {result}");
      }
    };

    foreach (var module in _commands.Modules)
    {
      await Logger.Log(LogSeverity.Info, $"{nameof(CommandHandler)} | Commands", $"Module '{module.Name}' initialized.");
    }
  }

  private async Task HandleCommandAsync(SocketMessage arg)
  {
    // Bail out if it's a System Message.
    if (arg is not SocketUserMessage msg)
      return;

    // We don't want the bot to respond to itself or other bots.
    if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
      return;

    var context = new ShardedCommandContext(_client, msg);

    var markPos = 0;
    if (msg.HasCharPrefix('.', ref markPos))
    {
      if (!msg.Content.StartsWith(".t")) return;

      var isShortTavisCommand = msg.Content.StartsWith(Constant.TavisCmdShorthand);
      var isTavisCommand = msg.Content.StartsWith(Constant.TavisCmd);

      if (!isShortTavisCommand && !isTavisCommand) return;

      if (isShortTavisCommand) await _commands.ExecuteAsync(context, Constant.TavisCmdShorthand.Length, Bootstrapper.ServiceProvider);
      else if (isTavisCommand) await _commands.ExecuteAsync(context, Constant.TavisCmd.Length, Bootstrapper.ServiceProvider);
    }
  }
}
