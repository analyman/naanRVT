namespace NaaN.dynamo.Geometry
{
    public class CSLine
    {
        private CSPoint firstPoints;
        private CSPoint secondPoints;

        private CSLine(){}

        public CSLine(CSPoint spt)
        {
            this.secondPoints = spt;
            this.firstPoints  = CSPoint.CSPointOrigin;
        }
    }
}