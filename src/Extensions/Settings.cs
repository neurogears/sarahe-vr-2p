//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.9.0.0 (Newtonsoft.Json v9.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace AutomaticGenerators
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class Trial
    {
private int corridorType = 0;
public int CorridorType
{
    get { return corridorType; }
    set { corridorType = value; }
}

        public PassiveMode PassiveMode { get; set; }

        public StimulusTextures StimulusTextures { get; set; }

        public Reward Reward { get; set; }

        public LedStimulation LedStimulation { get; set; }

        public Teleport Teleport { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class Json
    {
        public string Metadata { get; set; }

        public System.Collections.Generic.IDictionary<string, Trial> Trials { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class PassiveMode
    {
private bool enable = true;
public bool Enable
{
    get { return enable; }
    set { enable = value; }
}

private double gain = 0D;
public double Gain
{
    get { return gain; }
    set { gain = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StimulusTextures
    {
        public StimulusA StimulusA { get; set; }

        public StimulusB StimulusB { get; set; }

        public StimulusC StimulusC { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class Reward
    {
private bool enable = true;
public bool Enable
{
    get { return enable; }
    set { enable = value; }
}

private double location = 0D;
public double Location
{
    get { return location; }
    set { location = value; }
}

private double delay = 0D;
public double Delay
{
    get { return delay; }
    set { delay = value; }
}

private double amountHigh = 0D;
public double AmountHigh
{
    get { return amountHigh; }
    set { amountHigh = value; }
}

private double amountLow = 0D;
public double AmountLow
{
    get { return amountLow; }
    set { amountLow = value; }
}

private double probabilityHigh = 1D;
public double ProbabilityHigh
{
    get { return probabilityHigh; }
    set { probabilityHigh = value; }
}

private double probabilityLow = 1D;
public double ProbabilityLow
{
    get { return probabilityLow; }
    set { probabilityLow = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class LedStimulation
    {
private bool enable = false;
public bool Enable
{
    get { return enable; }
    set { enable = value; }
}

private double location = 0D;
public double Location
{
    get { return location; }
    set { location = value; }
}

private double delay = 0D;
public double Delay
{
    get { return delay; }
    set { delay = value; }
}

private int waveformIndex = 0;
public int WaveformIndex
{
    get { return waveformIndex; }
    set { waveformIndex = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class Teleport
    {
private bool enable = true;
public bool Enable
{
    get { return enable; }
    set { enable = value; }
}

private double location = 10D;
public double Location
{
    get { return location; }
    set { location = value; }
}

private double destination = 0D;
public double Destination
{
    get { return destination; }
    set { destination = value; }
}

private bool lockAtLocation = false;
public bool LockAtLocation
{
    get { return lockAtLocation; }
    set { lockAtLocation = value; }
}

private double lockAtLocationDuration = 0D;
public double LockAtLocationDuration
{
    get { return lockAtLocationDuration; }
    set { lockAtLocationDuration = value; }
}

private bool enableTeleportOnLick = false;
public bool EnableTeleportOnLick
{
    get { return enableTeleportOnLick; }
    set { enableTeleportOnLick = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StimulusA
    {
private string leftWall = "Assets\\Textures\\null.png";
public string LeftWall
{
    get { return leftWall; }
    set { leftWall = value; }
}

private string rightWall = "Assets\\Textures\\null.png";
public string RightWall
{
    get { return rightWall; }
    set { rightWall = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StimulusB
    {
private string leftWall = "Assets\\Textures\\null.png";
public string LeftWall
{
    get { return leftWall; }
    set { leftWall = value; }
}

private string rightWall = "Assets\\Textures\\null.png";
public string RightWall
{
    get { return rightWall; }
    set { rightWall = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StimulusC
    {
private string leftWall = "Assets\\Textures\\null.png";
public string LeftWall
{
    get { return leftWall; }
    set { leftWall = value; }
}

private string rightWall = "Assets\\Textures\\null.png";
public string RightWall
{
    get { return rightWall; }
    set { rightWall = value; }
}



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
