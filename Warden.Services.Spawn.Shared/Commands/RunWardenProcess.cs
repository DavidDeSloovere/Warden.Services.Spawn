using System;
using Warden.Common.Commands;

namespace Warden.Services.Spawn.Shared.Commands
{
    public class RunWardenProcess : ICommand
    {
        public Request Request { get; set; }
        public string ConfigurationId { get; set; }
        public string Token { get; set; }
    }
}