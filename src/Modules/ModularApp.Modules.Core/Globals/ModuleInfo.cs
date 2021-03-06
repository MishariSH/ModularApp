﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModularApp.Modules.Core.Globals
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public Assembly Assembly { get; set; }

        public string ShortName
        {
            get
            {
                return Name.Split('.').Last();
            }
        }

        public string Path { get; set; }
    }
}
