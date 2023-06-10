
using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using YamlDotNet.Serialization;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using AutomaticGenerators;

namespace AutomaticGenerators
{
    [Combinator]
    [Description("Constructor.")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public partial class TeleportProtocol
    {
        public IObservable<TeleportProtocol> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new TeleportProtocol
                {
					Destination = Destination,
					EndTrialOnTeleport = EndTrialOnTeleport,
					Location = Location,
					LockAtLocation = LockAtLocation,
					LockAtLocationDuration = LockAtLocationDuration,
					TeleportOnLick = TeleportOnLick,

                };
                return Observable.Return(value);
            });
        }
    }
}
