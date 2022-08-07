﻿namespace WebApplication1.Areas.Identity.Data
{
    public class UserAccounts
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string Amount { get; set; }
        public string Identifier { get; set; }
    }

    public class UploadDataDto
    {
        public List<string> DataColumns { get; set;}
    }

    public class AccountData
    {
        public string Account { get; set; }
        public List<string> Amount { get; set; }
        
    }
}
