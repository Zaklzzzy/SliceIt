
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения

        //Stats
        public int level = 0;
        public int money = 123500;
        // UnlockableItems
            public bool[] unlockKnifes = new bool[8];
            public bool[] unlockSliceable = new bool[8];
            public bool[] unlockWorlds = new bool[8];
        // Picked Item
        public int pickedKnife = 0;
        public int pickedObjects = 0;
        public int pickedWorld = 0;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {

        }
    }
}
