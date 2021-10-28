namespace DayZTediratorToolz.Helpers
{
    public class SafePosMapCoordinate
    {
        public SafePosMapCoordinate(int xcoord = 0, int ycoord = 0, int zcoord = 0)
        {
            Xcoord = xcoord;
            Ycoord = ycoord;
            Zcoord = zcoord;
        }

        public int Xcoord { get; set; }
        public int Ycoord { get; set; }
        public int Zcoord { get; set; }
    }
}