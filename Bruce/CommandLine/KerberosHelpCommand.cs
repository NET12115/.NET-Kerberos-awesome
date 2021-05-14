﻿// -----------------------------------------------------------------------
// Licensed to The .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kerberos.NET.Configuration;

namespace Kerberos.NET.CommandLine
{
    [CommandLineCommand("help", Description = "KerberosHelp")]
    public class KerberosHelpCommand : BaseCommand
    {
        public KerberosHelpCommand(CommandLineParameters parameters)
            : base(parameters)
        {
        }

        public static string Version
        {
            get
            {
                return Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion
                    .ToString();
            }
        }

        [CommandLineParameter("command", Description = "HelpCommand", EnforceCasing = false, FormalParameter = true)]
        public string Command { get; set; }

        public override Task<bool> Execute()
        {
            var comm = CommandLoader.CreateCommandExecutor(this.Command, null, ((ICommand)this).IO);

            if (comm == null)
            {
                if (!string.IsNullOrWhiteSpace(this.Command))
                {
                    this.WriteLine();
                    this.WriteLine(SR.Resource("CommandLine_UnknownCommand", this.Command));
                }

                this.DisplayUserDefaults();

                this.ListCommands();
            }
            else
            {
                this.DisplayUserDefaults();
            }

            return Task.FromResult(true);
        }

        private void DisplayUserDefaults()
        {
            this.WriteLine();
            this.WriteHeader(string.Format("{0}", SR.Resource("CommandLine_Defaults")));
            this.WriteLine();

            var props = new List<(string, string)>()
            {
                (SR.Resource("CommandLine_Version"), Version),
                (SR.Resource("CommandLine_ConfigPath"), Krb5Config.DefaultUserConfiguration),
                (SR.Resource("CommandLine_CachePath"), Krb5Config.DefaultUserCredentialCache),
            };

            var max = props.Max(p => p.Item1.Length) + 3;

            foreach (var prop in props)
            {
                this.WriteProperty(prop.Item1, prop.Item2, max);
            }
        }

        private void WriteProperty(string key, string value, int padding)
        {
            this.WriteLine(string.Format("{0}: {{Value}}",
                key.PadLeft(padding).PadRight(padding)), value);
        }

        private void ListCommands()
        {
            var types = CommandLoader.LoadTypes().OrderBy(t => t.GetCustomAttribute<CommandLineCommandAttribute>().Command);

            this.WriteLine();
            this.WriteHeader(string.Format("{0}", SR.Resource("CommandLine_Commands")));
            this.WriteLine();

            var max = types.Max(t => t.GetCustomAttribute<CommandLineCommandAttribute>().Command.Length) + 10;

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<CommandLineCommandAttribute>();

                var label = attr.Command.PadLeft(attr.Command.Length + 3).PadRight(max);

                var descName = "CommandLine_" + attr.Description;
                var desc = SR.Resource(descName);

                if (string.Equals(descName, desc, StringComparison.OrdinalIgnoreCase))
                {
                    this.WriteLine(string.Format("{0}{{Label}}", label), attr.Description);
                }
                else
                {
                    this.WriteLine(string.Format("{0}{{Label}}", label), desc);
                }
            }

            this.WriteLine();
        }
    }
}
