using System;
using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace ProtectCommand
{
    [ApiVersion(2, 1)]
    public class ProtectCommandPlugin : TerrariaPlugin
    {
        public override string Name => "Protect Command";
        public override string Author => "Keyou";
        public override string Description => "A plugin to restrict command usage to specific regions.";
        public override Version Version => new Version(1, 0, 0);

        private static Config config;

        public ProtectCommandPlugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            PlayerHooks.PlayerCommand += OnPlayerCommand;
            GeneralHooks.ReloadEvent += Reload;
        }

        private void OnInitialize(EventArgs args)
        {
            config = Config.Read();
        }

        private void Reload(ReloadEventArgs args)
        {
            config = Config.Read();
        }

        private void OnPlayerCommand(PlayerCommandEventArgs args)
        {
            if (!config.Enable)
            {
                return;
            }

            if (TShockAPI.Commands.ChatCommands.Find(c => c.HasAlias(args.CommandName)) == null)
            {
                return;
            }

            if (args.Player.Index == -1 || args.Player.HasPermission("protectcommand.bypass"))
            {
                return;
            }

            var player = TShock.Players[args.Player.Index];
            if (player == null)
            {
                return;
            }

            var commandName = args.CommandName.ToLower();
            var command = TShockAPI.Commands.ChatCommands.Find(c => c.HasAlias(args.CommandName));
            if (command != null)
            {
                commandName = command.Name.ToLower();
            }
            if (!config.ProtectedCommands.ContainsKey(commandName))
            {
                return;
            }

            ProtectedCommand(args);
            args.Handled = true;
        }

        private void ProtectedCommand(PlayerCommandEventArgs args)
        {
            var player = args.Player;
            var commandName = args.CommandName.ToLower();
            var command = TShockAPI.Commands.ChatCommands.Find(c => c.HasAlias(args.CommandName));
            if (command != null)
            {
                commandName = command.Name.ToLower();
            }
            
            if (!config.ProtectedCommands.TryGetValue(commandName, out var regionName))
            {
                player.SendErrorMessage("This command is not protected.");
                return;
            }

            var region = TShock.Regions.GetRegionByName(regionName);
            if (region == null || !region.InArea((int)(player.X / 16), (int)(player.Y / 16)))
            {
                player.SendErrorMessage(config.NotInRegionMessage.Replace("{region}", regionName));
                return;
            }

            PlayerHooks.PlayerCommand -= OnPlayerCommand;
            try
            {
                TShockAPI.Commands.HandleCommand(player, "/" + args.CommandText);
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"Error executing protected command: {ex}");
                player.SendErrorMessage("An error occurred while executing the command.");
            }
            finally
            {
                PlayerHooks.PlayerCommand += OnPlayerCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                PlayerHooks.PlayerCommand -= OnPlayerCommand;
            }
            base.Dispose(disposing);
        }
    }
}