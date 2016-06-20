using System;
using Microsoft.SPOT;
using GHI.Glide;
using GHI.Glide.UI;

namespace GadgeteerTwitter
{
    class FrameGHI
    {
        protected GHI.Glide.Display.Window window;
        protected Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolorLed;
        protected TextBlock txtCantidad;
        
        public FrameGHI(Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolor, Resources.StringResources resource)
        {
            this.window = GlideLoader.LoadWindow(Resources.GetString(resource));
            this.txtCantidad = (TextBlock)window.GetChildByName("txtCantidad");
            this.multicolorLed = multicolor;
            this.ContadorTwit = 0;
        }

        public int ContadorTwit 
        {
            get;
            set;
        }

        protected virtual void Parpadeo()
        {
            this.multicolorLed.BlinkRepeatedly(Gadgeteer.Color.Blue, new TimeSpan(0, 0, 1), Gadgeteer.Color.Green, new TimeSpan(0, 0, 1));
        }
        
        public void Show()
        {
            Parpadeo();
            this.txtCantidad.Text = "Usted tiene " + ContadorTwit + " menciones en twits";
            Glide.MainWindow = window;
        }
    }
}
