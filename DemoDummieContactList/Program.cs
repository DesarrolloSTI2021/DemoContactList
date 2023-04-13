using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDummieContactList
{
    internal class Program
    {
        private static List<Dictionary<string, object>> registros;
        private static List<WritableDialerContact> writableDialerContacts;
        private static OutboundApi outboundApi;
        static int contador = 0;
        static void Main(string[] args)
        {
            registros = new List<Dictionary<string, object>>();
            string contactlitId = "4270873c-014e-4fe3-b97c-0407e2e13bee";
            foreach (var item in GenerarDatosDummies())
            {
                contador++;
                AgregarDataListaContactos(item.NombreDummie, item.TelDummie);
                if (contador == 1000)
                {
                    contador = 0;
                    CargarRegistrosListaContactos(contactlitId);
                    registros.Clear();

                }
            }

            if (contador < 1000)
            {
                CargarRegistrosListaContactos(contactlitId);
                registros.Clear();
            }
        }

        public static List<DummieClass> GenerarDatosDummies()
        {
            List<DummieClass> list = new List<DummieClass>();

            list.Add(new DummieClass() { NombreDummie = "Prueba", TelDummie = "09812555444" });
            list.Add(new DummieClass() { NombreDummie = "Prueba", TelDummie = "09810000000" });
            list.Add(new DummieClass() { NombreDummie = "Prueba", TelDummie = "09811111111" });
            list.Add(new DummieClass() { NombreDummie = "Prueba", TelDummie = "09810111114" });

            return list;
        }

        static  AuthTokenInfo GenerarToken()
        {

            // Configuration.Default.ShouldRefreshAccessToken = false;
            var authTokenInfo = Configuration.Default.ApiClient.PostToken(
            "682ee29b-299f-495e-925c-85e86364f3aa",
            "A7MifSIZPgbDLFRi3M-mO5TYvKv0-r3QPhFhjBVdBZ8");

            return authTokenInfo;
        }

        static void AgregarDataListaContactos(string nombre,string tel)
        {

            Dictionary<string, object> diccionarioContacto = new Dictionary<string, object>();
            diccionarioContacto.Add("NombreDummie", nombre);
            diccionarioContacto.Add("TelDummie", tel);
            registros.Add(diccionarioContacto);


        }

        public static List<DialerContact> CargarRegistrosListaContactos(string contactlitId)
        {

            var token=GenerarToken();
            Configuration configuration= new Configuration()
                {
                    AccessToken = token.AccessToken
                };

            outboundApi = new OutboundApi(configuration);
            var dialerContactList = new List<WritableDialerContact>();

            foreach (var reg in registros)
            {
                WritableDialerContact dialerContact;

                Dictionary<string, PhoneNumberStatus> phonecallables = new Dictionary<string, PhoneNumberStatus>();

               //aqui validar si los teléfonos son llamable o no

                dialerContact = new WritableDialerContact("", contactlitId, reg, true, phonecallables);

                dialerContactList.Add(dialerContact);
            }

            var contacts = outboundApi.PostOutboundContactlistContacts(contactlitId, dialerContactList);

            return contacts;
        }
    }
}
