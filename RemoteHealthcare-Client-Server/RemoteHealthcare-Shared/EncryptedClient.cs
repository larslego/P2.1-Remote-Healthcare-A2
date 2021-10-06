﻿using CommClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RemoteHealthcare_Shared
{
    public class EncryptedClient : EncryptedSenderReceiver
    {
        public EncryptedClient(NetworkStream network)
        {
            Trace.WriteLine("EncryptedClient() called");
            Debug.WriteLine($"sslStream: {base.sslStream}");
            base.sslStream = new SslStream(network, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try
            {
                Trace.WriteLine("Authenticating");
                base.sslStream.AuthenticateAsClient("localhost");
                Trace.WriteLine("Client: Authenticated");
            } 
            catch (AuthenticationException e)
            {
                Trace.WriteLine($"Exception: {e.Message}");
                if (e.InnerException != null)
                {
                    Trace.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
                Trace.WriteLine("Authentication failed - closing the connection.");
                base.sslStream.Close();
                return;
            }
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Debug.WriteLine($"Certificate error: {sslPolicyErrors}");

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}
