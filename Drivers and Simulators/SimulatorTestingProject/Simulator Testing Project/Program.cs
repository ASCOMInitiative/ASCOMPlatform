namespace Simulator_Testing_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            object[] parameter = new object[1]; 

            //Type a = Type.GetTypeFromProgID("ASCOM.Simulator.RheostatSwitch");
            ////Create instance of excel 
            //RheostatSwitch  b = (RheostatSwitch) Activator.CreateInstance(a);
            ////Set the parameter whic u want to set 
            //parameter[0] = true;
            ////Set the Visible property 
            //a.InvokeMember("Visible", BindingFlags.SetProperty, null, b, parameter);

            var chooser = new ASCOM.Utilities.Chooser { DeviceType = "Switch" };
            chooser.Choose();
        }
    }
}
