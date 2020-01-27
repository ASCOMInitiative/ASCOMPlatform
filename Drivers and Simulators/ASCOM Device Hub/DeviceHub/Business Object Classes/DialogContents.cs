namespace ASCOM.DeviceHub
{
	public class DialogContents
	{
		public string ViewClassname { get; set; }
		public string ViewModelClassname { get; set; }

		public DialogContents( string viewClassname, string viewModelClassname )
		{
			ViewClassname = viewClassname;
			ViewModelClassname = viewModelClassname;
		}
	}

}
