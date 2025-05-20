﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class Measurement : BaseEntity
    {
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Neck { get; set; }
        public float Coat { get; set; }
        public float ChestAround { get; set; }
        public float Stomach { get; set; }
        public float ShoulderWidth { get; set; }
        public float Hip { get; set; }
    }
}
