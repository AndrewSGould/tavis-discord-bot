﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TavisBot.Common;
using TavisBot.Init;
using TavisBot.Services;
using dotenv.net;

var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));

var discordClientConfig = new DiscordSocketConfig()
{
  // TODO: We really don't need AllUnpriviledged, lets be responsible
  GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.AllUnprivileged
};

var client = new DiscordShardedClient(discordClientConfig);

var commands = new CommandService(new CommandServiceConfig
{
  LogLevel = LogSeverity.Info,
  CaseSensitiveCommands = false,
});

Bootstrapper.Init();
Bootstrapper.RegisterInstance(client);
Bootstrapper.RegisterInstance(commands);
Bootstrapper.RegisterType<ICommandHandler, CommandHandler>();
Bootstrapper.RegisterInstance(builder);

await MainAsync();

async Task MainAsync()
{
  await Bootstrapper.ServiceProvider!.GetRequiredService<ICommandHandler>().InitializeAsync();

  client.ShardReady += async shard =>
  {
    await Logger.Log(LogSeverity.Info, "ShardReady", $"Shard Number {shard.ShardId} is connected and ready!");
  };

  var envVars = DotEnv.Read();
  var token = envVars["DISCORD_BOT_TOKEN"];

  if (string.IsNullOrWhiteSpace(token))
  {
    await Logger.Log(LogSeverity.Error, $"{nameof(Program)} | {nameof(MainAsync)}", "Token is null or empty.");
    return;
  }

  await client.LoginAsync(TokenType.Bot, token);
  await client.StartAsync();

  // Wait infinitely so the bot stays connected
  await Task.Delay(Timeout.Infinite);
}
