using Malignant.Common;
using Malignant.Common.Systems;
using Terraria.ModLoader;

namespace Malignant
{
    public class Malignant : Mod
	{
        public static Malignant Mod { get; set; }

        public Malignant()
        {
            Mod = this;
        }

        public override void Load()
        {
            MalignantLists.LoadLists();
            PrayerContent.Load(Mod);
            SoundManager.Load(Mod);
        }

        public override void Unload()
        {
            MalignantLists.UnloadLists();
            Mod = null;
        }
    }
}
