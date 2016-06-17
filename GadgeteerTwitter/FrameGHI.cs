using System;
using Microsoft.SPOT;
using GHI.Glide;

namespace GadgeteerTwitter
{
    class FrameGHI
    {
        protected GHI.Glide.Display.Window window;
        protected Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolorLed;
        
        public FrameGHI(Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolor, Resources.StringResources resource)
        {
            this.window = GlideLoader.LoadWindow(Resources.GetString(resource));
            this.multicolorLed = multicolor;
        }

        protected virtual void Parpadeo()
        {
            this.multicolorLed.BlinkRepeatedly(Gadgeteer.Color.Blue, new TimeSpan(0, 0, 1), Gadgeteer.Color.Green, new TimeSpan(0, 0, 1));
        }
        
        public void Show()
        {
            Parpadeo();
            Glide.MainWindow = window;
        }
    }
}
