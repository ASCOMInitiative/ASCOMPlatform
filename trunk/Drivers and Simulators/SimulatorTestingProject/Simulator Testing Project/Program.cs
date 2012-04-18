namespace Simulator_Testing_Project
{
    class Program
    {
        static void Main(string[] args)
        {

            ASCOM.DriverAccess.Telescope scope = new ASCOM.DriverAccess.Telescope("ASCOM.Simulator.Telescope");
            scope.Connected = true;
            System.Console.WriteLine("Position " + scope.RightAscension.ToString() + ", " + scope.Declination.ToString());

            System.Console.ReadLine();
            //object[] parameter = new object[1]; 

            object s = System.Activator.GetObject(System.Type.GetTypeFromProgID("ASCOM.Simulator.Telescope"),string.Empty);

            //Type a = Type.GetTypeFromProgID("ASCOM.Simulator.RheostatSwitch");
            ////Create instance of excel 
            //RheostatSwitch  b = (RheostatSwitch) Activator.CreateInstance(a);
            ////Set the parameter whic u want to set 
            //parameter[0] = true;
            ////Set the Visible property 
            //a.InvokeMember("Visible", BindingFlags.SetProperty, null, b, parameter);

            //var chooser = new ASCOM.Utilities.Chooser { DeviceType = "Switch" };
            //chooser.Choose();
        }
    }
}
