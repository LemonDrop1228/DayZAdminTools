using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DayZTediratorToolz.Helpers;

namespace DayZTediratorToolz.Models
{
    public class RecentTypesHistoryModel
    {
        public RecentTypesHistoryModel InitializeEmpty()
        {
            TypeHistory = new(3);
            return this;
        }

        public void UpdateHistory(string typesFilePath)
        {
            TypeHistory.Add(new RecentTypeHistoryItemModel(){RecentPath = typesFilePath});
        }

        CircularFIFO<RecentTypeHistoryItemModel> TypeHistory { get; set; }
    }

    public class RecentTypeHistoryItemModel
    {
        public string RecentPath { get; set; }
    }


}