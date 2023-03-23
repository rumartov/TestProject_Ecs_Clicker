using Data;

namespace Services
{
    public interface ISaveLoadService
    {
        void Save(PlayerProgress progress);
        PlayerProgress Load();
        PlayerProgress NewProgress();
    }
}