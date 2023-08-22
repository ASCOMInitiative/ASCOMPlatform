namespace ASCOM.DynamicRemoteClients
{
    public class RateResponse
    {
        private double maximum = 0.0;
        private double minimum = 0.0;

        public RateResponse(double minimum, double maximum)
        {
            this.maximum = maximum;
            this.minimum = minimum;
        }

        public double Maximum
        {
            get { return this.maximum; }
            set { this.maximum = value; }
        }

        public double Minimum
        {
            get { return this.minimum; }
            set { this.minimum = value; }
        }
    }
}
