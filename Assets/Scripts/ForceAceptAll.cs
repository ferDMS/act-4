using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class ForceAceptAll : CertificateHandler
{
    // Función para ignorar el certificado SSL de una llamada a HTTPS
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
