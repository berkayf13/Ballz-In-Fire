namespace ThirdParty.Tools.Serializablecallback.Runtime
{
	public abstract class InvokableCallbackBase<TReturn> {
		public abstract TReturn Invoke(params object[] args);
	}
}