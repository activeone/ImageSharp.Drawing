﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixLabors.Shapes.Tests
{
    public static class Extensions
    {
        public static IPath AsPath(this Rectangle rect)
        {
            return ((IPath)rect);
        }
    }
}
