using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;

namespace MReader.Core.Models
{
    public class ReaderState
    {
        public ControlSize AppWindowSize { get; set; }
        public double ReaderPanelWidth { get; set; }

        public ReaderState()
        {
            AppWindowSize = new ControlSize(525.0, 350.0);
            ReaderPanelWidth = double.NaN;
        }

        public ReaderState(double readerPlanelWidth, ControlSize appWindowSize)
        {
            ReaderPanelWidth = readerPlanelWidth;
            AppWindowSize = appWindowSize;
        }
    }
}
