using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Controls
{
    interface IStatusControl
    {
        event EventHandler OnSendData;
        event EventHandler OnSaveData;
        event EventHandler OnCambioDeEstado;

        /** Guardar Datos del Control */
        void SaveData();
        /** Enviar Datos (botón Enviar?) */
        void SendData();
    }
}
