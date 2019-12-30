﻿using bgTeam.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.bgTeam.Core.Tests
{
    public class HelpersTest
    {
        [Fact]
        public void ConfigHelper_Init()
        {
            var configFolderPath = Path.Combine(Environment.CurrentDirectory, "Configurations");
            var configurations = ConfigHelper.Init<InsuranceConfiguration>(configFolderPath);

            Assert.True(configurations.Any());
        }

        public class InsuranceConfiguration
        {
            public string ConfigName { get; set; }

            public string ContextType { get; set; }

            public string Description { get; set; }

            public string NameQueue { get; set; }

            public string DateFormatStart { get; set; }

            public int? DateChangeOffsetFrom { get; set; }

            public int? DateChangeOffsetTo { get; set; }

            public string[] Sql { get; set; }
        }

    }
}