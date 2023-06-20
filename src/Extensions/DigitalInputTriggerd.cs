using System;
using System.Reactive.Linq;
using OpenCV.Net;
using NationalInstruments.DAQmx;
using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bonsai.DAQmx
{
    /// <summary>
    /// Represents an operator that reads a sequence of logical values from
    /// one or more DAQmx digital input lines.
    /// </summary>
    [DefaultProperty(nameof(Channels))]
    [Description("Reads a sequence of logical values from one or more DAQmx digital input lines.")]
    public class DigitalInputTriggered : Source<Mat>
    {
        readonly Collection<DigitalInputChannelConfiguration> channels = new Collection<DigitalInputChannelConfiguration>();

        /// <summary>
        /// Gets or sets the optional source terminal of the clock. If not specified,
        /// the internal clock of the device will be used.
        /// </summary>
        [Description("The optional source terminal of the clock. If not specified, the internal clock of the device will be used.")]
        private string signalSource = string.Empty;
        public string SignalSource
        {
            get { return signalSource; }
            set { signalSource = value; }
        }

        /// <summary>
        /// Gets or sets the sampling rate for reading logical values, in samples
        /// per second.
        /// </summary>
        [Description("The sampling rate for reading logical values, in samples per second.")]
        public double SampleRate { get; set; }

        /// <summary>
        /// Gets or sets a value specifying on which edge of a clock pulse sampling takes place.
        /// </summary>
        [Description("Specifies on which edge of a clock pulse sampling takes place.")]
        private SampleClockActiveEdge activeEdge = SampleClockActiveEdge.Rising;
        public SampleClockActiveEdge ActiveEdge
        {
            get { return activeEdge; }
            set { activeEdge = value; }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the acquisition task will acquire
        /// a finite number of samples or if it continuously acquires samples.
        /// </summary>
        [Description("Specifies whether the acquisition task will acquire a finite number of samples or if it continuously acquires samples.")]
        private SampleQuantityMode sampleMode = SampleQuantityMode.ContinuousSamples;
        public SampleQuantityMode SampleMode
        {
            get { return sampleMode; }
            set { sampleMode = value; }
        }

        /// <summary>
        /// Gets or sets the number of samples to acquire, for finite samples, or the
        /// size of the buffer for continuous sampling.
        /// </summary>
        [Description("The number of samples to acquire, for finite samples, or the size of the buffer for continuous sampling.")]
        private int bufferSize = 1000;
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        /// <summary>
        /// Gets or sets the number of samples per channel in each output buffer.
        /// If not specified, the number of samples will be set to the size of the buffer.
        /// </summary>
        [Description("The number of samples in each output buffer. If not specified, the number of samples will be set to the size of the buffer.")]
        private int? samplesPerChannel;
        public int? SamplesPerChannel
        {
            get { return samplesPerChannel; }
            set { samplesPerChannel = value; }
        }

        /// <summary>
        /// Gets the collection of virtual input channels from which to read logical values.
        /// </summary>
        [Editor("Bonsai.Design.DescriptiveCollectionEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        [Description("The collection of virtual input channels from which to read logical values.")]
        public Collection<DigitalInputChannelConfiguration> Channels
        {
            get { return channels; }
        }

        Task CreateTask()
        {
            var task = new Task();
            foreach (var channel in channels)
            {
                task.DIChannels.CreateChannel(channel.Lines, channel.ChannelName, channel.Grouping);
            }

            return task;
        }

        /// <summary>
        /// Reads an observable sequence of logical values from one or more
        /// DAQmx digital input lines.
        /// </summary>
        /// <returns>
        /// A sequence of 2D <see cref="Mat"/> objects storing the logical values.
        /// Each row corresponds to a channel in the acquisition task, and each column
        /// to a sample from each of the channels. The order of the channels follows
        /// the order in which you specify the channels in the <see cref="Channels"/>
        /// property. Each sample can represent either a single line or a bitmask
        /// representing the state of all digital lines in a single port, depending
        /// on the configuration of the virtual channel.
        /// </returns>
        public override IObservable<Mat> Generate()
        {
            return Observable.Create<Mat>(observer =>
            {
                var task = CreateTask();
                var bufferSize = BufferSize;
                task.Timing.ConfigureSampleClock(SignalSource, SampleRate, ActiveEdge, SampleMode, bufferSize);
                task.Control(TaskAction.Verify);
                var digitalInReader = new DigitalMultiChannelReader(task.Stream);
                var samplesPerChannel = SamplesPerChannel.GetValueOrDefault(bufferSize);
                AsyncCallback digitalCallback = null;
                digitalCallback = new AsyncCallback(result =>
                {
                    var data = digitalInReader.EndReadMultiSamplePortByte(result);
                    var output = Mat.FromArray(data);
                    observer.OnNext(output);
                    digitalInReader.BeginReadMultiSamplePortByte(samplesPerChannel, digitalCallback, null);
                });

                digitalInReader.SynchronizeCallbacks = true;
                digitalInReader.BeginReadMultiSamplePortByte(samplesPerChannel, digitalCallback, null);
                return Disposable.Create(() =>
                {
                    task.Stop();
                    task.Dispose();
                });
            });
        }
    }
}