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
    [Description("Generates voltage signals in one or more DAQmx analog output channels from a sequence of sample buffers.")]
    public class CounterOutput : Source<Unit>
    {
        private string counterChannel = string.Empty;
        public string CounterChannel
        {
            get { return counterChannel; }
            set { counterChannel = value; }
        }

        private string triggerSource = string.Empty;
        public string TriggerSource
        {
            get { return triggerSource; }
            set { triggerSource = value; }
        }

        private double frequency = 1;
        public double Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        private double dutyCycle = 0.5;
        public double DutyCycle
        {
            get { return dutyCycle; }
            set { dutyCycle = value; }
        }

        private int? numberOfSamples = null;
        public int? NumberOfSamples
        {
            get { return numberOfSamples; }
            set { numberOfSamples = value; }
        }

        public override IObservable<Unit> Generate()
        {
            return Observable.Using(
                () => new Task(),
                task => Observable.Create<Unit>(observer =>
                {
                    task.COChannels.CreatePulseChannelFrequency(
                        counterChannel,
                        "",
                        COPulseFrequencyUnits.Hertz,
                        COPulseIdleState.Low,
                        0, frequency, dutyCycle);
                    if (numberOfSamples.HasValue)
                    {
                        task.Timing.ConfigureImplicit(SampleQuantityMode.FiniteSamples, numberOfSamples.Value);
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