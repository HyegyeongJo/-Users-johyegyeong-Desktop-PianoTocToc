using ToryValue;

namespace ToryFramework.Input
{
	[System.Serializable]
	public class ToryInteractionTypeEnum : ToryEnum<InteractionType>
	{
		protected ToryInteractionTypeEnum()
		{
		}

		public ToryInteractionTypeEnum(string key) : this(key, default(InteractionType))
		{
		}

		public ToryInteractionTypeEnum(string key, InteractionType value) : base(key, value)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryInteractionTypeEnum(string key, InteractionType value, InteractionType defaultValue) : base(key, value, defaultValue)
		{
		}

		[System.Obsolete("This constructor is obsolete.")]
		public ToryInteractionTypeEnum(string key, InteractionType value, InteractionType defaultValue, InteractionType savedValue) : base(key, value, defaultValue, savedValue)
		{
		}

		public ToryInteractionTypeEnum(string key, InteractionType value, System.Func<InteractionType, InteractionType> setterFunc)
			: base(key, value, setterFunc)
		{
		}

		public ToryInteractionTypeEnum(string key, InteractionType value, System.Func<InteractionType, InteractionType> setterFunc, System.Func<InteractionType, InteractionType> getterFunc)
			: base(key, value, setterFunc, getterFunc)
		{
		}
	}
}