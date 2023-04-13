namespace App.Logic_Components
{
    public interface ICell
    {
        public delegate void CellOperation(ICell cell);
        public event CellOperation OnInteraction;

        public CellValue Value { get; set; }
    }
}
