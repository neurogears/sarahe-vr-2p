using System;
using System.Reactive.Linq;
using OpenCV.Net;
using NationalInstruments.DAQmx;
using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bonsai.DAQmx
{
    public class RotaryEncoderInput : Source<int>
    {
        private string channel;

        [TypeConverter(typeof(CounterInputPhysicalChannelConverter))]
        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        private CIEncoderDecodingType decoderType = CIEncoderDecodingType.X1;
        public CIEncoderDecodingType DecoderType
        {
            get { return decoderType; }
            set { decoderType = value; }
        }

        private int ticksPerRevolution = 1000;
        public int TicksPerRevolution
        {
            get { return ticksPerRevolution; }
            set { ticksPerRevolution = value; }
        }

        private double startingRadians = 0;
        public double StartingRadians
        {
            get { return startingRadians; }
            set { startingRadians = value; }
        }

        private bool resetOnZ = false;
        public bool ResetOnZ
        {
            get { return resetOnZ; }
            set { resetOnZ = value; }
        }

        Task CreateTask()
        {
            var task = new Task();

            task.CIChannels.CreateAngularEncoderChannel(
                channel,
                "RotaryEncoderInput",
                decoderType,
                resetOnZ,
                0,
                CIEncoderZIndexPhase.AHighBHigh,
                ticksPerRevolution,
                startingRadians,
                CIAngularEncoderUnits.Radians);

            return task;
        }


        class CounterInputPhysicalChannelConverter : StringConverter
        {
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(DaqSystem.Local.GetPhysicalChannels(
                    PhysicalChannelTypes.CI,
                    PhysicalChannelAccess.External));
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        public override IObservable<int> Generate()
        {
            throw new NotImplementedException();
        }

        public IObservable<int> Generate<TSource>(IObservable<TSource> source)
        {

            return Observable.Defer(() =>
            {
                var task = CreateTask();
                task.Control(TaskAction.Verify);
                task.Control(TaskAction.Commit);
                var counterInReader = new CounterReader(task.Stream);
                task.Start();
                return Observable.Using(() => Disposable.Create(
                    () =>
                    {
                        task.Stop();
                        task.Dispose();
                    }),
                    resource => source.Select(_ =>
                    {
                        var data = counterInReader.ReadSingleSampleInt32();
                        return data;
                    }));
            });
        }
    }
}