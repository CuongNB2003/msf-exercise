﻿
namespace MsfServer.Application.Contracts.Statistic.Dto
{
    public class StatisticLogMethodByMonth
    {
        public DateTime AccessDate { get; set; }
        public int PostCount { get; set; }
        public int PutCount { get; set; }
        public int GetCount { get; set; }
        public int DeleteCount { get; set; }
    }
}