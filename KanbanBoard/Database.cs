using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace KanbanBoard
{
    public static class Database
    {
        const string DbPath = @".\database.db";

        public static LiteDatabase Connect()
        {
            return new LiteDatabase(DbPath);
        }
    }
}
