using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.Entities;
using HackAtHome.SAL;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName")]
    public class ListEvidenciasActivity : Activity
    {

        Persistencia Data;
        List<Evidence> Evidencias = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListaEvidencias);

            var ListEvidencias = FindViewById<ListView>(Resource.Id.listView1);
            string Token = Intent.GetStringExtra("token");
            string Usuario = Intent.GetStringExtra("usuario") ?? "No Disponible";

            FindViewById<TextView>(Resource.Id.textView5).Text = Usuario;

            //Intentar recuperar el fragmento
            Data = (Persistencia)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                cargarEvidencias(ListEvidencias, Token);
                // No ha sido almacenado, agregar el fragmento a la Activity
                Data = new Persistencia();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
                
            }else
            {
                ListEvidencias.Adapter = new CustomAdapters.EvidencesAdapter(this, Data.Evidencias, 
                    Resource.Layout.ListItem,
                    Resource.Id.textView1, Resource.Id.textView2);
            }

            
         
          

            ListEvidencias.ItemClick += (sender, e) =>
            {
                var item = Evidencias[e.Position];
                int idEvidencia = item.EvidenceID;
                string TituloEvidencia = item.Title;
                string StatusEvidencia = item.Status;

                var DetalleEvActivity = new Android.Content.Intent(this, typeof(DetalleEvidenciaActivity));
                DetalleEvActivity.PutExtra("usuario", Usuario);
                DetalleEvActivity.PutExtra("token", Token);
                DetalleEvActivity.PutExtra("idEvidencia", idEvidencia);
                DetalleEvActivity.PutExtra("tituloEvidencia", TituloEvidencia);
                DetalleEvActivity.PutExtra("StatusEvidencia", StatusEvidencia);
                StartActivity(DetalleEvActivity);
            };

        }

        public async void cargarEvidencias(ListView ListaEvidencias, string token)
        {

            HackAtHome.SAL.ServiceClient Serv = new HackAtHome.SAL.ServiceClient();
            Evidencias = await Serv.GetEvidencesAsync(token);
            Data.Evidencias = Evidencias;
            ListaEvidencias.Adapter = new CustomAdapters.EvidencesAdapter(this, Evidencias, Resource.Layout.ListItem,
                    Resource.Id.textView1, Resource.Id.textView2);
        }

        public async void SendEv(string emailtext)
        {
            var MicrosoftEvidence = new LabItem
            {
                Email = emailtext,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)

            };
            var MicrosoftClient = new MicrosoftServiceClient();
            await MicrosoftClient.SendEvidence(MicrosoftEvidence);
        }
    }
}