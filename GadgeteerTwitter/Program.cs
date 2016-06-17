using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHI.Glide;

namespace GadgeteerTwitter
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        private FrameTwitterDormido windowD;
        private FrameTwitterFeliz windowF;
        private FrameTwitterTriste windowT;
        private int MINIMO_TWITEES = 3;
        private static int contador = 0;
        private static byte[] ipserver = new byte[4] {192, 168, 0, 10};
        private enum ESTADO { ACTIVO, APAGADO };
        private ESTADO estado = ESTADO.APAGADO;
        void ProgramStarted()
        {
            Debug.Print("Program Started");
            initialize_ethernet();

            this.windowD = new FrameTwitterDormido(this.multicolorLED);
            this.windowT = new FrameTwitterTriste(this.multicolorLED);
            this.windowF = new FrameTwitterFeliz(this.multicolorLED);
            
            this.windowD.BtnTapEvent += windowD_BtnEvent;
            this.windowD.BtnTapReset += windowD_BtnTapReset;
            this.windowF.BtnEvent += windowF_BtnEvent;
            this.windowT.BtnEvent += windowT_BtnEvent;

            ethernetJ11D.NetworkUp += new GTM.Module.NetworkModule.NetworkEventHandler(ethernetJ11D_NetworkUp);
            ethernetJ11D.NetworkDown += new GTM.Module.NetworkModule.NetworkEventHandler(ethernetJ11D_NetworkDown);
            this.windowD.Show();
            
            GlideTouch.Initialize();
        }

        /*private void RunWebServer()
        {
            while (ethernetJ11D.IsNetworkUp == false)
            {
                Debug.Print("Esperando...");
                Thread.Sleep(1000);
            }
            string ipAddress = ethernetJ11D.NetworkSettings.IPAddress;
            Debug.Print("IP STAT netowork up " + ipAddress);
            WebServer.StartLocalServer(ipAddress, 80);
            WebEvent twit = WebServer.SetupWebEvent("twit");
            twit.WebEventReceived += new WebEvent.ReceivedWebEventHandler(twit_WebEventReceived);
            //WebServer.DefaultEvent.WebEventReceived += DefaultEvent_WebEventReceived;
        }*/

        public int ContadorTwit
        {
            get
            {
                return contador;
            }
            set
            {
                contador = value;
                this.windowD.ContadorTwit = value;
            }
        }
        private void twit_WebEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            Debug.Print("Entro a twit_WebEventReceived" + contador);
            ContadorTwit += 1;
            /*string content = "<html><body><h1>Hello World!!</h1></body></html>";
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(content);
            responder.Respond(bytes, "text/html");*/
        }

        private void ethernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("No hay cable de red");
        }

        private void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            string ipAddress = ethernetJ11D.NetworkSettings.IPAddress;
            Debug.Print("IP STAT netowork up " + ipAddress);
            /*WebServer.StartLocalServer(ethernetJ11D.NetworkSettings.IPAddress, 80);
            this.RunWebServer();
            Thread.Sleep(1);*/
            SocketServer server = new SocketServer(ipserver, 8080);
            server.DataReceived += new DataReceivedEventHandler(server_DataReceived);
            server.Start();
        }

        private void server_DataReceived(object sender, DataReceivedEventArgs e)
        {
            ContadorTwit += 1;
            if (ESTADO.ACTIVO.Equals(estado))
            {
                if (contador > MINIMO_TWITEES)
                {
                    this.windowF.Show();
                }
                else
                {
                    this.windowT.Show();
                }
            }
            else
            {
                this.windowD.Show();
            }
        }

        private void initialize_ethernet()
        {
            Debug.Print("initialize_ethernet");
            ethernetJ11D.NetworkInterface.Open();
            ethernetJ11D.NetworkInterface.EnableDhcp();
            ethernetJ11D.UseThisNetworkInterface();
            //Debug.Print("IP STATIC 192.168.65.17 ");
            //ethernetJ11D.UseStaticIP("192.168.65.17", "255.255.255.0", "192.168.65.254");
            String ip = "" + ipserver[0] + "." + ipserver[1] + "." + ipserver[2] + "." + ipserver[3];
            ethernetJ11D.UseStaticIP(ip, "255.255.255.0", "192.168.0.1");
            //ethernetJ11D.UseDHCP();
        }

        private void windowT_BtnEvent(object sender)
        {
            this.estado = ESTADO.APAGADO;
            this.windowD.Show();
        }

        private void windowF_BtnEvent(object sender)
        {
            this.estado = ESTADO.APAGADO;
            this.windowD.Show();
        }

        private void windowD_BtnEvent(object sender)
        {
            this.estado = ESTADO.ACTIVO;
            this.windowT.Show();
        }

        void windowD_BtnTapReset(object sender)
        {
            this.estado = ESTADO.APAGADO;
            ContadorTwit = 0;
            this.windowD.Show();
        }
    }
}
