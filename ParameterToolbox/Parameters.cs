using System;
using System.Collections;
using System.Collections.Generic;

namespace ParameterToolbox
{
    public class Parameters
    {
        public DateTime EndDate { get; set; }
        public IList Funds { get; set; } = new List<Fund>();
        public DateTime StartDate { get; set; }
    }
}