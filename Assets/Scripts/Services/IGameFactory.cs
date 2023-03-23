using Components;

namespace Services
{
    public interface IGameFactory
    {
        public Balance Balance { get; set; }
        public void CreateBalance();
        public void CreateBusinessCards();
    }
}