using System;
using System.Reactive.Linq;
using OpenCV.Net;
using NationalInstruments.DAQmx;
using System.Reactive.Disposables;
using System.ComponentModel;
using System.Reactive;

namespace Bonsai.DAQmx
{
    /// <summary>
    /// Represents an operator that generates voltage signals in one or more DAQmx analog
    /// output channels from a sequence of sample buffers.
    /// </summary>
    [DefaultProperty("Channel")]
    [Description("Generate a Pulse on a counter output using the specified time specifications (in seconds).")]
    public class PulseOutput : Source<Unit>
    {

        private string channel;

        [TypeConverter(typeof(CounterOutputPhysicalChannelConverter))]
        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        class CounterOutputPhysicalChannelConverter : StringConverter
        {
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(DaqSystem.Local.GetPhysicalChannels(
                    PhysicalChannelTypes.CO,
                    PhysicalChannelAccess.External));
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }

        private double initialDelay = 0;
        public double InitialDelay
        {
            get { return initialDelay; }
            set { initialDelay = value; }
        }

        private double lowTime = 0;
        public double LowTime
        {
            get { return lowTime; }
            set { lowTime = value; }
        }

        private double highTime = 0;
        public double HighTime
        {
            get { return highTime; }
            set { highTime = value; }
        }

        private int? NumberOfPulses = 1;
        public int? NumberOfSamples
        {
            get { return NumberOfPulses; }
            set { NumberOfPulses = value; }
        }

        public override IObservable<Unit> Generate()
        {
            return Observable.Using(
                () => new Task(),
                task => Observable.Create<Unit>(observer =>
                {
                    task.COChannels.CreatePulseChannelTime(
                        channel,
                        "",
                        COPulseTimeUnits.Seconds,
                        COPulseIdleState.Low,
                        InitialDelay, LowTime, HighTime);
                    if (NumberOfPulses.HasValue)
                    {
                        task.Timing.ConfigureImplicit(SampleQuantityMode.FiniteSamples, NumberOfPulses.Value);
                    }
                    else
                    {
                        task.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples);
                    }
                    task.Control(TaskAction.Verify);
                    task.Control(TaskAction.Commit);

                    task.Done += (sender, e) =>
                    {
                        if (e.Error != null) observer.OnError(e.Error);
                        else observer.OnCompleted();
                    };
                    task.Start();

                    return Disposable.Create(task.Stop);
                }
            )
            );
        }
    }
}