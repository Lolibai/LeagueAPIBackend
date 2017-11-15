﻿using System;
namespace LeagueAPI.Models
{
    public class ISummoner
    {
        public int profileIconId { get; set; }
        public string name { get; set; }
        public int summonerLevel { get; set; }
        public int accountId { get; set; }
        public int id { get; set; }
        public long revisionDate { get; set; }
    }
}
