using ToryValue;

namespace ToryFramework
{
	public partial class ToryValue
	{
		public ToryFloat BGMVolume { get { return ValueBehaviour.BGMVolume; } set { ValueBehaviour.BGMVolume = value; }}
		public ToryFloat SFXVolume { get { return ValueBehaviour.SFXVolume; } set { ValueBehaviour.SFXVolume = value; }}
	}
}