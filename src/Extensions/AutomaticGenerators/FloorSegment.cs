
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
    public partial class FloorSegment
    {
        public IObservable<FloorSegment> Process()
        {
            return Observable.Defer(() =>
            {
                var value = new FloorSegment
                {
					InitialVisibility = InitialVisibility,
					Name = Name,
					PositionEnd = PositionEnd,
					PositionStart = PositionStart,
					Texture = Texture,
					AdditionalProperties = AdditionalProperties,

                };
                return Observable.Return(value);
            });
        }
    }
}
