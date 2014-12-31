using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi;

namespace InforWhats
{
    class WhatsApi:EventosArgs
    {
        public WhatsApi()
        {
            this.WaApp = new WhatsApp();
        }
    }
}
