//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Connection class
//
// Description:	This implements a telescope connection class for Gemini
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 14-AUG-2010	pk  1.0.0a	Initial edit -- created to support Gemini II ethernet 
//                          interface
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Timers;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
using ASCOM.GeminiTelescope.Properties;
using System.Net;
using System.IO;
using System.Web;

namespace ASCOM.GeminiTelescope
{

    public partial class GeminiHardwareBase
    {
        private  string EthernetResult = "";
        private  HttpWebResponse Response = null;

        private Int32 PacketCount = 1;
     

        internal byte [] UDP_datagram1 = new byte[1024];
        internal byte [] UDP_datagram2 = new byte[1024];
        internal int UDP_len_lastdatagram = 0;
        internal byte [] UDP_buff = null;


        /// <summary>
        /// Previous request response never arrived.
        /// Request a resend from Gemini (NACK) and then
        /// decide if the packet was lost on the way to Gemini or on the way back
        /// If on the way back, resend the original request packet.
        /// </summary>
        internal string RetransmitUDP()
        {
            Trace.Enter("RetransmitUDP");
            if (!this.Connected) return null;    //no resync if we are not already connected

            // if no previous packet, don't resync
            if (UDP_len_lastdatagram == 0)
            {
                Trace.Exit("RetransmitUDP", "no previous packet!");
                return null;
            }

            int expected_seq_num = BitConverter.ToInt32(UDP_buff, 0); // this is what the reply shouxeld contain as the sequence number

            // extract last command sent:
            string last_cmd = m_BinaryEncoding.GetString(UDP_buff, 8, UDP_len_lastdatagram - 9);

            Trace.Info(2, "RetransmitUDP", expected_seq_num, last_cmd);

            // try maximum number of times. If no response, we are done. Connection is lost.
            for (int trynbr = 0; trynbr < SharedResources.MAXIMUM_ERRORS; ++trynbr)
            {
                Trace.Info(2, "RetransmitUDP:try", trynbr, last_cmd);

                //if (m_AllowErrorNotify)
                //{
                //    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, "Connection lost...Retrying");
                //}

                if (trynbr > 0)
                {
                    // since we didn't get a proper response, try to reset the socket and see if can get a response:
                    GeminiHardware.Instance.ResyncEthernet();
                }

                DiscardInBuffer();
                DiscardOutBuffer();

                int resend_seq_num = UDPSendNACK(); // ask Gemini to resend last packet it sent to us

                Trace.Info(3, "RetransmitUDP:SentNACK", resend_seq_num);

                byte[] res = null;
                int len = getUDPCommandResult(1000, ref res);


                Trace.Info(3, "RetransmitUDP:GotNACK", len, res);

                // got a response back, check if valid:
                if (len > 0 && res!=null)
                {
                    int seq1 = BitConverter.ToInt32(res, 0);
                    int seq2 = BitConverter.ToInt32(res, 4);

                    Trace.Info(3, "RetransmitUDP:GotNACK2", seq1, seq2);

                    
                    // seq1 should be the same as our resend packet seq #, if not, ignore this and try again
                    if (seq1 != resend_seq_num) {
                        Trace.Info(3, "RetransmitUDP:GotNACK", "seq1# doesn't match");
                        System.Threading.Thread.Sleep(500); // wait a little
                        continue; //unexpected response sequence #, ignore it, and try again
                    }
                   
                    // got a response to our NACK resend request, check if the lost command needs to be resent!
                    if (seq2 != expected_seq_num) // Gemini never received the command, resend it
                    {
                        Trace.Info(3, "RetransmitUDP:GotNACK", "seq2# doesn't match, will resend cmd");

                        //resend it
                        TransmitUDP(last_cmd);

                        string cmd_res = getUDPCommandResult(1000);

                        Trace.Info(2, "RetransmitUDP:Got result:", cmd_res, last_cmd);

                        // got a proper response, return it!
                        if (cmd_res != null)
                        {
                            //if (m_AllowErrorNotify)
                            //{
                            //    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, "Connection re-established!");
                            //}
                            return cmd_res;
                        }
                        System.Threading.Thread.Sleep(500); // wait a little and retry again
                    }
                    else
                    {
                        //if (m_AllowErrorNotify)
                        //{
                        //    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, "Connection re-established!");
                        //}

                        // Success! Received a matching packet from Gemini. Return the contents as the request result:
                        string s = m_BinaryEncoding.GetString(UDP_buff, 8, UDP_len_lastdatagram - 9);
                        Trace.Info(2, "RetransmitUDP:Received", s);
                    }

                }
            }

            Trace.Exit("RetransmitUDP", "Failed to retransmit!");
            return null;    // no luck, give up.
        }

        private void TransmitUDP(string s)
        {

            UDP_buff = (UDP_buff==UDP_datagram1? UDP_datagram2 : UDP_datagram1);
            Array.Copy(System.BitConverter.GetBytes(PacketCount), 0, UDP_buff, 0, 4);
            Trace.Info(2, "TransmitUDP packet#, cmd", PacketCount, s);

            PacketCount++;
            Array.Copy(m_BinaryEncoding.GetBytes(s), 0, UDP_buff, 8, s.Length);
            UDP_buff[s.Length + 8] = 0;
            UDP_len_lastdatagram = s.Length + 9;
            GeminiHardware.Instance.UDP_client.Send(UDP_buff, UDP_len_lastdatagram, GeminiHardware.Instance.UDP_endpoint);
        }
       
        private void TransmitUDP(byte[] cmd)
        {
            UDP_buff = (UDP_buff == UDP_datagram1 ? UDP_datagram2 : UDP_datagram1);
            Array.Copy(System.BitConverter.GetBytes(PacketCount), 0, UDP_buff, 0, 4);
            Trace.Info(2, "TransmitUDP packet#, cmd", PacketCount, cmd);
            PacketCount++; 
            Array.Copy(cmd, 0, UDP_buff, 8, cmd.Length);
            UDP_len_lastdatagram = cmd.Length + 8;
            GeminiHardware.Instance.UDP_client.Send(UDP_buff, UDP_len_lastdatagram, GeminiHardware.Instance.UDP_endpoint);
        }
        
        
        byte[] rbuff = new byte[9];

        /// <summary>
        /// Request a UDP retransmit of last packet sent
        ///   this uses a private buffer, so as not to overwrite previously sent datagram data
        ///   
        /// returns actual packet sequence number that was sent.
        /// </summary>
        private int UDPSendNACK()
        {
            Array.Copy(System.BitConverter.GetBytes(PacketCount), 0, rbuff, 0, 4);
            Trace.Info(2, "ReTransmitUDP packet#, cmd", PacketCount);
            rbuff[8] = 0x15;
            GeminiHardware.Instance.UDP_client.Send(rbuff, 9, GeminiHardware.Instance.UDP_endpoint);
            return PacketCount++;
        }

        private void TransmitEthernet(string s)
        {
            if (GeminiHardware.Instance.UDP)
            {
                lock (m_SerialPort)
                {
                    try
                    {
                        TransmitUDP(s);
                    }
                    catch {
                        GeminiHardware.Instance.ResyncEthernet();
                        TransmitUDP(s);
                    }
                }
                return;
            }

             Trace.Enter(4, "TransmitE", s);

            EthernetResult = "";
            
            if (Response != null) Response.Close();
            Response = null;

            if (s == String.Empty) return;

            string url = UseDHCP?
                "http://" + GeminiDHCPName + "/ser.cgx?SE=" + s :
                "http://" + EthernetIP + "/ser.cgx?SE=" + s;

            url = url.Replace("#", "%23");

            m_SerialErrorOccurred.Reset();

            Trace.Info(4, "Before Ethernet Transmit", s);
            lock (m_SerialPort)
            {
                Trace.Info(0, "Ethernet Transmit", s);

                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];
                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(url);

                request.Headers = new WebHeaderCollection();
                request.Headers.Add("Accept-Language: en-US");
                if (BypassProxy) request.Proxy = null;

                request.Credentials = new NetworkCredential(EthernetUser, EthernetPassword);
                request.PreAuthenticate = true;
                request.Timeout = MAX_TIMEOUT;


                try
                {
                    Response = (HttpWebResponse)request.GetResponse();
//                    response.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("401"))
                    {
                        if (m_AllowErrorNotify)
                        {
                            Trace.Except(ex);
                            if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, "Invalid Gemini user-id or password.");
                        }
                        m_SerialErrorOccurred.Set();
                    }
                    else
                    {
                        m_SerialErrorOccurred.Set();
                    }
                }

                Trace.Info(4, "Finished Transmit", s);
            }

           

            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Tramsmit timeout", s);
                throw new TimeoutException("Transmission error: " + s);
            }
            Trace.Exit(4, "TransmitE");
        }


        private void TransmitSerial(string s)
        {
            Trace.Enter(4, "Transmit", s);

            if (s == String.Empty) return;

            m_SerialErrorOccurred.Reset();

            if (m_SerialPort.IsOpen)
            {
                Trace.Info(4, "Before Serial Transmit", s);
                lock (m_SerialPort)
                {
                    Trace.Info(0, "Serial Transmit", s);
                    m_SerialPort.Write(m_BinaryEncoding.GetBytes(s), 0, s.Length);
                    m_SerialPort.BaseStream.Flush();
                    Trace.Info(4, "Finished Port.Write");
                }
            }
            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Tramsmit timeout", s);
                throw new TimeoutException("Serial port transmission error: " + s);
            }
            Trace.Exit(4, "Transmit");
        }


        private string GetCommandResult(CommandItem command, bool bResyncOnError)
        {
            bool bReturn = false;
            return GetCommandResult(command, bResyncOnError, ref bReturn);
        }

                /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// 
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <param name="bResyncOnError">true if driver should resync if an error was detected</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private string GetCommandResult(CommandItem command, bool bResyncOnError, ref bool bReturn)
        {
            if (m_EthernetPort)
                return GetCommandResultEthernet(command, bResyncOnError, ref bReturn);
            else
                return GetCommandResultSerial(command, bResyncOnError, ref bReturn);
        }




        private string getEthernetCommandResult(int timeout)
        {
            if (GeminiHardware.Instance.UDP)
                return getUDPCommandResult(timeout);

            if (Response == null) return null;

            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // we will read data via the response stream
            Stream resStream = Response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = m_BinaryEncoding.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            Response.Close();
            Response = null;
            return sb.ToString();
        }

        IPEndPoint iep0 = new IPEndPoint(0, 11110);
        Byte[] receiveBytes = null;

        /// <summary>
        /// Synchronous get UDP datagram function
        ///   sleeps until data is available or a timeout occurs
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal string getUDPCommandResult(int timeout)
        {
wait_again:
            m_DataReceived.Reset();

            IAsyncResult res = GeminiHardware.Instance.UDP_client.BeginReceive(new AsyncCallback(UDPReceiveData), null);
            if (m_DataReceived.WaitOne(timeout))
            {
                if (res.IsCompleted)
                {
                    receiveBytes = GeminiHardware.Instance.UDP_client.EndReceive(res, ref iep0);
                    string data = m_BinaryEncoding.GetString(receiveBytes, 8, receiveBytes.Length - 9);
                    Int32 seq2 = BitConverter.ToInt32(receiveBytes,0);
                    Int32 seq1 = BitConverter.ToInt32(UDP_buff,0);
                    if (seq1 != seq2) 
                        goto wait_again;

                    Trace.Info(2, "getUDPCommandResult received", data);
                    return data;
                }
            }
            Trace.Info(2, "getUDPCommandResult timeout");
            // do packet recovery logic to determine why we didn't get a response           
            return RetransmitUDP();

        }

        /// <summary>
        /// Synchronous get UDP datagram function
        ///   sleeps until data is available or a timeout occurs
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal int getUDPCommandResult(int timeout, ref byte[] result)
        {
            m_DataReceived.Reset();

            IAsyncResult res = GeminiHardware.Instance.UDP_client.BeginReceive(new AsyncCallback(UDPReceiveData), null);
            if (m_DataReceived.WaitOne(timeout))
            {
                if (res.IsCompleted)
                {
                    Byte[] receiveBytes = GeminiHardware.Instance.UDP_client.EndReceive(res, ref iep0);
                    Trace.Info(2, "getUDPCommandResult received", receiveBytes);
                    result = receiveBytes;
                    return result.Length;
                }
            }
            return 0;

        }




        void UDPReceiveData(IAsyncResult ar)
        {
            m_DataReceived.Set();
        }

        /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// 
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <param name="bResyncOnError">true if driver should resync if an error was detected</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private string GetCommandResultEthernet(CommandItem command, bool bResyncOnError, ref bool bReturn)
        {
            string result = null;
            Trace.Enter(4, "GetCommandResultE", command.m_Command);

            GeminiCommand.ResultType gemini_result = GeminiCommand.ResultType.HashChar;
            int char_count = 0;
            GeminiCommand gmc = FindGeminiCommand(command.m_Command);

            if (gmc != null)
            {
                gemini_result = gmc.Type;
                char_count = gmc.Chars;
                command.m_UpdateRequired = gmc.UpdateStatus;
            }
           
            // no result expected by this command, just return;
            if (gemini_result == GeminiCommand.ResultType.NoResult)
            {
                bReturn = false;
                return null;
            }
            
            bReturn = true;

            m_SerialTimeoutExpired.Reset();
            m_SerialErrorOccurred.Reset();

            int timeout = command.m_Timeout;


            try
            {
                Trace.Info(0, "Ethernet wait for response", command.m_Command);

                string res  = "";

                if (EthernetResult.Length == 0)
                {
                    res = getEthernetCommandResult(timeout);

                    if (res == null && EthernetResult.Length == 0)
                        throw new Exception("Ethernet connection error");
                    if (res != null)
                        EthernetResult += res;
                }

                switch (gemini_result)
                {
                    // a specific number of characters expected as the return value
                    case GeminiCommand.ResultType.NumberofChars:
                        result = ReadNumberE(char_count);
                        break;

                    // value '1' or a string terminated by '#'
                    case GeminiCommand.ResultType.OneOrHash:
                        result = ReadNumberE(1); ;  // check if first character is 1, and return if it is, no hash expected
                        if (result != "1")
                        {
                            result += ReadToE('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.ZeroOrHash:
                        result = ReadNumberE(1);
                        if (result != "0")
                        {
                            result += ReadToE('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // string terminated by '#'
                    case GeminiCommand.ResultType.HashChar:
                        result = ReadToE('#');
                        if (command.m_Raw) //Raw should return the full string including #
                            result += "#";
                        break;

                    // '0' or two strings, each terminated with '#' (:SC command)
                    case GeminiCommand.ResultType.ZeroOrTwoHash:
                        result = ReadNumberE(1);
                        if (result != "0")
                        {
                            result += ReadToE('#');
                            if (command.m_Raw) result += '#';
                            result += ReadToE('#');
                            if (command.m_Raw) result += '#';
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (m_AllowErrorNotify)
                {
                    Trace.Except(ex);
                    GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Timeout error occurred after " + command.m_Timeout + "msec while processing command '" + command.m_Command + "'");
                    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.EthernetTimeout);
                }

                AddOneMoreError();
                if (bResyncOnError) Resync();
                return null;
            }
            finally
            {
                tmrReadTimeout.Stop();
            }


            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Ethernet port error", command.m_Command);

                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Comm error reported while processing command '" + command.m_Command + "'");
                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.EthernetError);
                AddOneMoreError();
                return null;  // error occurred!
            }

            if (result != null)
                m_LastDataTick = DateTime.Now;      // remember when last successfull data was received.

            // return value for native commands has a checksum appended: validate it and remove it from the return string:
            if (!string.IsNullOrEmpty(result) && (command.m_Command[0] == '<' || command.m_Command[0] == '>') && !command.m_Raw)
            {
                char chksum = result[result.Length - 1];
                result = result.Substring(0, result.Length - 1); //remove checksum character

                if ((((int)chksum) & 0x7f) != (ComputeChecksum(result) & 0x7f))  // bad checksum -- ignore the return value! 
                {
                    Trace.Error("Bad Checksum", command.m_Command, result);

                    if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.EthernetError);
                    AddOneMoreError();
                    result = null;
                }
            }

            Trace.Info(0, "Ethernet received:", command.m_Command, result);
            
            if (!string.IsNullOrEmpty(result)) result = DeEscape(result);

            Trace.Exit(4, "GetCommandResultEthernet", command.m_Command, result);
            return result;
        }

        static Regex regexUnicode = new Regex(@"\&\^([0-9]+)\;", RegexOptions.Compiled);

        /// <summary>
        /// Translates G2 escaped Unicode characters into real characters
        /// </summary>
        /// <param name="EscapedString">The string contraining one or more escaped characters</param>
        /// <returns>The supplied string with escaped charcters converted to real characters.</returns>
        /// <remarks>G2 supplies extended Unicode characters in the form &^XXX; where X is a decimal number of arbitary length as required. This routine 
        /// uses a regular expression to remove  the encoded character string, extract the decimal part, convert this to a character and subsititute this
        /// for the original encoded character string.</remarks>
        string DeEscape(string EscapedString)
        {
            string DeEscapedString = EscapedString;
            try
            {
                DeEscapedString = regexUnicode.Replace(EscapedString, match => ((char)int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.Number)).ToString());
            }
            catch (Exception ex)
            {
                Trace.Info(0, "DeEscape Exception: ", ex.ToString());
            }

            return DeEscapedString;
        }

        /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// 
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <param name="bResyncOnError">true if driver should resync if an error was detected</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private string GetCommandResultSerial(CommandItem command, bool bResyncOnError, ref bool bReturn)
        {
            string result = null;

            if (!m_SerialPort.IsOpen) return null;

            Trace.Enter(4, "GetCommandResult", command.m_Command);

            GeminiCommand.ResultType gemini_result = GeminiCommand.ResultType.HashChar;
            int char_count = 0;
            GeminiCommand gmc = FindGeminiCommand(command.m_Command);

            if (gmc != null)
            {
                gemini_result = gmc.Type;
                char_count = gmc.Chars;
                command.m_UpdateRequired = gmc.UpdateStatus;
            }

            // no result expected by this command, just return;
            if (gemini_result == GeminiCommand.ResultType.NoResult)
            {
                bReturn = false;
                return null;
            }
            
            bReturn = true;

            m_SerialTimeoutExpired.Reset();
            m_SerialErrorOccurred.Reset();

            if (command.m_Timeout > 0)
            {
                tmrReadTimeout.Interval = command.m_Timeout;
                tmrReadTimeout.Start();
            }

            try
            {
                Trace.Info(0, "Serial wait for response", command.m_Command);

                switch (gemini_result)
                {
                    // a specific number of characters expected as the return value
                    case GeminiCommand.ResultType.NumberofChars:
                        result = ReadNumber(char_count);
                        break;

                    // value '1' or a string terminated by '#'
                    case GeminiCommand.ResultType.OneOrHash:
                        result = ReadNumber(1); ;  // check if first character is 1, and return if it is, no hash expected
                        if (result != "1")
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.ZeroOrHash:
                        result = ReadNumber(1);
                        if (result != "0")
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // string terminated by '#'
                    case GeminiCommand.ResultType.HashChar:
                        result = ReadTo('#');
                        if (command.m_Raw) //Raw should return the full string including #
                            result += "#";
                        break;

                    // '0' or two strings, each terminated with '#' (:SC command)
                    case GeminiCommand.ResultType.ZeroOrTwoHash:
                        result = ReadNumber(1);
                        if (result != "0")
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) result += '#';
                            result += ReadTo('#');
                            if (command.m_Raw) result += '#';
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (m_AllowErrorNotify)
                {
                    Trace.Except(ex);
                    GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Timeout error occurred after " + command.m_Timeout + "msec while processing command '" + command.m_Command + "'");
                    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialTimeout);
                }

                AddOneMoreError();

                if (bResyncOnError) Resync();


                return null;
            }
            finally
            {
                tmrReadTimeout.Stop();
            }


            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Communications error", command.m_Command);

                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Comm error reported while processing command '" + command.m_Command + "'");
                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialError);
                AddOneMoreError();
                return null;  // error occurred!
            }

            if (result != null)
                m_LastDataTick = DateTime.Now;      // remember when last successfull data was received.

            // return value for native commands has a checksum appended: validate it and remove it from the return string:
            if (!string.IsNullOrEmpty(result) && (command.m_Command[0] == '<' || command.m_Command[0] == '>') && !command.m_Raw)
            {
                char chksum = result[result.Length - 1];
                result = result.Substring(0, result.Length - 1); //remove checksum character

                if ((((int)chksum) & 0x7f) != (ComputeChecksum(result) & 0x7f))  // bad checksum -- ignore the return value! 
                {
                    Trace.Error("Bad Checksum", command.m_Command, result);

                    GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Serial comm error (bad checksum) while processing command '" + command.m_Command + "'");
                    if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialError);
                    AddOneMoreError();
                    result = null;
                }
            }

            Trace.Info(0, "Serial received:", command.m_Command, result);
            Trace.Exit(4, "GetCommandResult", command.m_Command, result);
            return result;
        }

        /// <summary>
        /// Add one more error to the total error tally
        /// if number of errors in the defined interval (MAXIMUM_ERROR_INTERVAL) exceeds specified number (MAXIMUM_ERRORS)
        ///   assume that Gemini is off-line or some other catastrophic failure has occurred.
        ///   Send a message to the user through OnError event to fix the problem,
        ///   reset pending communication queues, and wait a defined "cool-down" interval of (RECOVER_SLEEP)
        ///   then, resume processing.
        /// </summary>
        internal void AddOneMoreError()
        {
            Trace.Enter("AddOneMoreError", m_TotalErrors);

            if (!Connected || m_BackgroundWorker == null || !m_BackgroundWorker.IsAlive) return;    //not ready yet
            if (IgnoreErrors) return;   // driver asked us not to report errors, perhaps Gemini is too busy to respond during a slew or a flip...

            if (Connected && DateTime.Now - m_LastDataTick > TimeSpan.FromSeconds(SharedResources.MAXIMUM_DISCONNECT_TIME))
            {
                string msg = "No response for " + (SharedResources.MAXIMUM_DISCONNECT_TIME).ToString() + " secs";
                Trace.Error(msg, "Teminating connection!");
                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, msg + " Terminating connection!");
                if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.TerminatingConnection);
                while (m_Connected) Disconnect();
                return;
            }

            // if this is the first error, or if it's been longer than maximum interval since the last error, start from scratch
            if (m_TotalErrors == 0)
                m_FirstErrorTick = System.Environment.TickCount;

            if (m_FirstErrorTick + SharedResources.MAXIMUM_ERROR_INTERVAL/5 < System.Environment.TickCount)
            {
                m_FirstErrorTick = System.Environment.TickCount;
                if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.EthernetError);
            }

            if (++m_TotalErrors > SharedResources.MAXIMUM_ERRORS)
            {
                Trace.Error("Too many errors");

                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.TooManyErrors);
                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Too many serial port errors in the last " + SharedResources.MAXIMUM_ERROR_INTERVAL / 1000 + " seconds. Resetting serial port.");

                lock (m_CommandQueue) // remove all pending commands, keep the queue locked so that the worker thread can't process during port reset
                {
                    Trace.Info(0, "Resetting communications port");

                    m_CommandQueue.Clear();
                    DiscardInBuffer();
                    if (!m_EthernetPort)
                    {
                        m_SerialPort.DiscardOutBuffer();
                        try
                        {
                            Trace.Info(2, "Closing port");
                            m_SerialPort.Close();
                            System.Threading.Thread.Sleep(SharedResources.RECOVER_SLEEP);

                            Trace.Info(0, "Opening port");
                            m_SerialPort.Open();
                            m_SerialPort.Encoding = m_BinaryEncoding;
                        }
                        catch (Exception ex)
                        {
                            Trace.Except(ex);
                            GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Cannot reset serial port after errors: " + ex.Message);
                        }
                    }
                }
                m_TotalErrors = 0;
                m_FirstErrorTick = 0;
            }
            //else Resync();

            Trace.Exit("AddOneMoreError");
        }



        void tmrReadTimeout_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_SerialTimeoutExpired.Set();
        }

        /// <summary>
        /// Read serial port until the terminating character is encoutered. Don't include 
        /// terminating character in the result, honor readtimeout specified on the port.
        /// </summary>
        /// <param name="terminate"></param>
        /// <returns></returns>
        private string ReadTo(char terminate)
        {
            StringBuilder res = new StringBuilder();

            StringBuilder outp = new StringBuilder();

            for (; ; )
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    int b = m_SerialPort.ReadByte();
                    char c = (char)(b);

                    if (c != terminate)
                    {
                        // 223 = degree character, the only char > 0x80 that's used in normal
                        // response to commands (longitude, latitude, etc.) 
                        // it must occur inside the string to be a legitimate response,
                        // otherwise consider it part of a binary stream meant for the passthrough port
                        if ((int)c >= 0x80 && (c != 223 || res.Length == 0)) outp.Append(c);
                        else
                            res.Append(c);
                    }
                    else
                    {
                        if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                            m_PassThroughPort.PassStringToPort(outp);
                        return res.ToString();
                    }
                }
                if (m_SerialTimeoutExpired.WaitOne(0))
                {
                    if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    throw new TimeoutException("ReadTo");
                }

                //                    System.Threading.Thread.Sleep(0);  //[pk] should instead wait on a waithandle set by serialdatareceived event...
                try { m_DataReceived.WaitOne(250); }
                catch { }
            }
        }

        /// <summary>
        /// Read exact number of characters from the serial port, honoring the read timeout
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private string ReadNumber(int chars)
        {
            StringBuilder res = new StringBuilder();
            StringBuilder outp = new StringBuilder();

            for (; ; )
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    byte c = (byte)m_SerialPort.ReadByte();
                    if ((int)c >= 0x80)
                        outp.Append(Convert.ToChar(c));
                    else
                        res.Append(Convert.ToChar(c));

                    if (res.Length == chars)
                    {
                        if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                            m_PassThroughPort.PassStringToPort(outp);
                        return res.ToString();
                    }
                }
                if (m_SerialTimeoutExpired.WaitOne(0))
                {
                    if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    throw new TimeoutException("ReadNumber");
                }
                //                    System.Threading.Thread.Sleep(0);   //[pk] should instead wait on a waithandle set by serialdatareceived event...
                try { m_DataReceived.WaitOne(250); }
                catch { }
            }
        }


        /// <summary>
        /// Read serial port until the terminating character is encoutered. Don't include 
        /// terminating character in the result, honor readtimeout specified on the port.
        /// </summary>
        /// <param name="terminate"></param>
        /// <returns></returns>
        private string ReadToE(char terminate)
        {
            StringBuilder res = new StringBuilder();

            StringBuilder outp = new StringBuilder();

            while (EthernetResult.Length > 0)
            {
                int b = EthernetResult[0];
                EthernetResult = EthernetResult.Remove(0, 1);
                
                char c = (char)(b);

                if (c != terminate)
                {
                    // 223 = degree character, the only char > 0x80 that's used in normal
                    // response to commands (longitude, latitude, etc.) 
                    // it must occur inside the string to be a legitimate response,
                    // otherwise consider it part of a binary stream meant for the passthrough port
                    if ((int)c >= 0x80 && (c != 223 || res.Length == 0)) outp.Append(c);
                    else
                        res.Append(c);
                }
                else
                {
                    if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    return res.ToString();
                }
            }
            if (m_SerialTimeoutExpired.WaitOne(0))
            {
                if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                    m_PassThroughPort.PassStringToPort(outp);
                throw new TimeoutException("ReadTo");
            }
            return res.ToString();
        }

        /// <summary>
        /// Read exact number of characters from the serial port, honoring the read timeout
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private string ReadNumberE(int chars)
        {
            StringBuilder res = new StringBuilder();
            StringBuilder outp = new StringBuilder();

            while (EthernetResult.Length > 0)
            {
                byte c = (byte)EthernetResult[0];
                EthernetResult = EthernetResult.Remove(0, 1);

                if ((int)c >= 0x80)
                    outp.Append(Convert.ToChar(c));
                else
                    res.Append(Convert.ToChar(c));

                if (res.Length == chars)
                {
                    if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    return res.ToString();
                }
            }
            if (m_SerialTimeoutExpired.WaitOne(0))
            {
                if (outp.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                    m_PassThroughPort.PassStringToPort(outp);
                throw new TimeoutException("ReadNumber");
            }
            return res.ToString();
        }



        internal void DiscardInBuffer()
        {
            if (EthernetPort) 
                DiscardInBufferEthernet();
            else 
                DiscardInBufferSerial();
        }


        internal void DiscardOutBuffer()
        {
            if (EthernetPort) ;
            else
                m_SerialPort.DiscardOutBuffer();
        }


        /// <summary>
        /// Discard anything in the in-buffer, but keep all the binary 
        /// data, as it may be needed by the software on the other side of the
        /// pass-through port
        /// </summary>
        /// 
        private void DiscardInBufferEthernet()
        {
            EthernetResult = "";
        }



        /// <summary>
        /// Discard anything in the in-buffer, but keep all the binary 
        /// data, as it may be needed by the software on the other side of the
        /// pass-through port
        /// </summary>
        /// 
        private void DiscardInBufferSerial()
        {
            StringBuilder sb = new StringBuilder();

            if (m_PassThroughPort != null && m_PassThroughPort.PortActive)
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    int c = m_SerialPort.ReadByte();
                    if (c >= 0x80) sb.Append(Convert.ToChar(c));
                }
                if (sb.Length > 0 && (m_PassThroughPort != null && m_PassThroughPort.PortActive))
                    m_PassThroughPort.PassStringToPort(sb);
            }
            else m_SerialPort.DiscardInBuffer();
        }


   }
}
