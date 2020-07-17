namespace ASCOM.DeviceHub
{
	public class SlewAmount
    {
		public SlewAmount( string name, double amount )
		{
			Name = name;
			Amount = amount;
		}

		public string Name { get; private set; }
		public double Amount { get; private set; }
	}
}
