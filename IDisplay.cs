﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Demo
{
    public interface IDisplay
    {
        void DisplayEquations(int dimension);
        void OutputResults(double[] result);
    }
}