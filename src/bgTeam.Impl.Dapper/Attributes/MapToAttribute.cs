﻿namespace bgTeam.DataAccess.Impl.Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MapToAttribute : Attribute
    {
        public MapToAttribute(string databaseColumn)
        {
            DatabaseColumn = databaseColumn;
        }

        public string DatabaseColumn { get; private set; }
    }
}
