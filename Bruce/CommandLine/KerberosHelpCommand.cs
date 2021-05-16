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
            var comm = CommandLoader.CreateCommandExecutor(this.Command, this.Parameters.WithCommand(this.Command), ((ICommand)this).IO);

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
                WriteLine();

                WriteCommandLabel(comm.GetType());

                this.DisplayUserDefaults();

                comm.DisplayHelp();
            }

            return Task.FromResult(true);
        }

        private void DisplayUserDefaults()
        {
            this.WriteLine();
            this.WriteHeader(SR.Resource("CommandLine_Defaults"));
            this.WriteLine();

            var props = new List<(string, string)>()
            {
                (SR.Resource("CommandLine_Version"), Version),
                (SR.Resource("CommandLine_ConfigPath"), Krb5Config.DefaultUserConfiguration),
                (SR.Resource("CommandLine_CachePath"), Krb5Config.DefaultUserCredentialCache),
            };

            WriteProperties(props);
        }

        private void ListCommands()
        {
            var types = CommandLoader.LoadTypes().OrderBy(t => t.GetCustomAttribute<CommandLineCommandAttribute>().Command);

            this.WriteLine();
            this.WriteHeader(SR.Resource("CommandLine_Commands"));
            this.WriteLine();

            var max = types.Max(t => t.GetCustomAttribute<CommandLineCommandAttribute>().Command.Length) + 10;

            foreach (var type in types)
            {
                WriteCommandLabel(type, max);
            }
        }

        private void WriteCommandLabel(Type type, int max = 0)
        {
            var attr = type.GetCustomAttribute<CommandLineCommandAttribute>();

            if (max <= 0)
            {
                max = attr.Command.Length + 4;
            }

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
    }
}
