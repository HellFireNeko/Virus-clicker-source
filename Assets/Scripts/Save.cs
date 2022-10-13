using System;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

namespace Scripts
{
    public class Save
    {
        const string SaveDataKey = "GameSave";

        public bool IsSavedataReset = true;
        public DateTime SaveDate;
        public double Money;

        public Dictionary<string, int> generators = new();

        public static void ResetData()
        {
            PlayerPrefs.SetString(SaveDataKey, JsonConvert.SerializeObject(new Save()));
            PlayerPrefs.Save();
        }

        public static Save LoadData()
        {
            if (!PlayerPrefs.HasKey(SaveDataKey)) ResetData();
            return JsonConvert.DeserializeObject<Save>(PlayerPrefs.GetString(SaveDataKey));
        }

        public static bool CheckResetStatus()
        {
            var save = LoadData();
            return save.IsSavedataReset;
        }

        public void SaveData()
        {
            this.SaveDate = DateTime.Now;
            IsSavedataReset = false;
            PlayerPrefs.SetString(SaveDataKey, JsonConvert.SerializeObject(this));
            PlayerPrefs.Save();
        }
    }
}
