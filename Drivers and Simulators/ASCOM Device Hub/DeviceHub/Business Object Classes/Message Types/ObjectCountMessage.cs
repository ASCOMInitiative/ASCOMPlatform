namespace ASCOM.DeviceHub
{
	public class ObjectCountMessage
	{
		public int ScopeCount { get; private set; }
		public int DomeCount { get; private set; }
		public int FocuserCount { get; private set; }

		public ObjectCountMessage( int scopeCount, int domeCount, int focuserCount )
		{
			ScopeCount = scopeCount;
			DomeCount = domeCount;
			FocuserCount = focuserCount;
		}
	}
}
