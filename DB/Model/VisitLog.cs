﻿using System;

namespace DB.Model
{
    public class VisitLog
    {
        public int Id { get; set; }
        public DateTime VisitDateTime { get; set; }

        public Pacient Pacient { get; set; }
        public Doctor Doctor { get; set; }
    }
}