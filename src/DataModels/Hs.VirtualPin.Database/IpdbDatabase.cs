﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hs.VirtualPin.Database
{
    public class IpdbDatabase : PinballXTable
    {
        public int Id { get; set; }

        public byte Players { get; set; }

        public string Abbreviation { get; set; }

        public int Units { get; set; }
    }
}
