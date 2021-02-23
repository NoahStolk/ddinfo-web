using DiscordBotDdInfo.Commands;
using DiscordBotDdInfo.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBotDdInfo
{
	public sealed class CommandHandler
	{
		private readonly List<string> _ddstatsCommands = new() { ".global", ".id", ".live", ".me", ".rank", ".register", ".search", ".top" };

		private readonly List<char> _prefixes = new() { '.', '!' };

		private readonly List<ulong> _channelIds = new()
		{
			ServerConstants.DdInfoChannelId,
			ServerConstants.CustomLeaderboardMonitoringChannelId,
			ServerConstants.ErrorMonitoringChannelId,
			ServerConstants.TaskMonitoringChannelId,
			ServerConstants.TestMonitoringChannelId,
		};

		private static readonly Lazy<CommandHandler> _lazy = new(() => new());

		private CommandHandler()
		{
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(AbstractCommand)))
			{
				AbstractCommand? commandInstance = (AbstractCommand?)Activator.CreateInstance(type);
				if (commandInstance == null)
					continue;
				CommandInstances.Add(commandInstance);
			}
		}

		public static CommandHandler Instance => _lazy.Value;

		public List<AbstractCommand> CommandInstances { get; } = new();

		public async Task MessageReceived(DiscordClient client, MessageCreateEventArgs e)
		{
			try
			{
				string msg = e.Message.Content.ToLower(CultureInfo.InvariantCulture);
				if (msg.Length <= 1)
					return;

				// React with an emoji when the bot gets mentioned anywhere.
				if (msg.Contains($"@!{ServerConstants.BotUserId}", StringComparison.InvariantCulture))
					await e.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":eye_in_speech_bubble:"));

				// Make sure the bot only posts it its own channels.
				if (!_channelIds.Contains(e.Channel.Id))
					return;

				// Intercept ddstats commands.
				if (_ddstatsCommands.Any(s => msg.StartsWith(s, StringComparison.InvariantCulture)))
				{
					await e.Channel.SendMessageAsyncSafe("That looks like a command ddstats understands. Did you intend to post this message in the #ddstats channel?");
					return;
				}

				// Handle the message.
				if (!_prefixes.Contains(msg[0]))
					return;

				string[] inputs = msg[1..].Split(' ');
				string commandName = inputs[0];
				string[] commandParameters = inputs[1..];

				AbstractCommand? command = CommandInstances.Find(t => t.Name.Contains(commandName, StringComparison.InvariantCulture));
				if (command == null)
				{
					await e.Channel.SendMessageAsyncSafe($"The command '{commandName}' does not exist. Type `.help` to see the list of currently available commands.");
					return;
				}

				IEnumerable<MethodInfo> executeMethods = command
					.GetType()
					.GetMethods(BindingFlags.Instance | BindingFlags.Public)
					.Where(m =>
						m.Name.Contains("Execute", StringComparison.InvariantCulture) &&
						(m.GetParameters().Length == commandParameters.Length || m.IsSearchCommand()));
				if (!executeMethods.Any())
				{
					await e.Channel.SendCommandResult(CommandResult.GetIncorrectUsage(command.GetType(), command.Name));
					return;
				}

				foreach (MethodInfo method in executeMethods)
				{
					ParameterInfo[] parameters = method.GetParameters();
					if (parameters.Length == 0)
					{
						await e.Channel.SendCommandResult(CommandResult.GetFromInvokedMethod(method.Invoke(command, null)));
						return;
					}
					else if (commandParameters.Length > 0 && method.IsSearchCommand())
					{
						await e.Channel.SendCommandResult(CommandResult.GetFromInvokedMethod(method.Invoke(command, new[] { string.Join(' ', commandParameters) })));
						return;
					}

					bool isValidMethod = true;
					int i = 0;
					List<object> convertedParameters = new();
					foreach (ParameterInfo parameter in parameters)
					{
						try
						{
							convertedParameters.Add(TypeDescriptor.GetConverter(parameter.ParameterType).ConvertFromString(commandParameters[i++]));
						}
						catch
						{
							isValidMethod = false;
							break;
						}
					}

					if (isValidMethod)
					{
						await e.Channel.SendCommandResult(CommandResult.GetFromInvokedMethod(method.Invoke(command, convertedParameters.ToArray())));
						return;
					}
					else
					{
						await e.Channel.SendCommandResult(CommandResult.GetIncorrectUsage(command.GetType(), command.Name));
						return;
					}
				}
			}
			catch (Exception ex)
			{
				DiscordEmbedBuilder builder = new()
				{
					Title = "An error occurred while handling the message",
					Color = DiscordColor.Red,
				};
				builder.AddError(ex);
				await e.Channel.SendMessageAsync(null, false, builder.Build());
			}
		}
	}
}
