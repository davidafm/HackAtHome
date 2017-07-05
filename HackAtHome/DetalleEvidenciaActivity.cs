using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Android.Graphics;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName")]
    public class DetalleEvidenciaActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DetalleEvidencia);

            CargarDetalle();

          
        }

        public async void CargarDetalle()
        {
            string Token = Intent.GetStringExtra("token");
            int idEv = Intent.GetIntExtra("idEvidencia", 0);

            FindViewById<TextView>(Resource.Id.textView1).Text = Intent.GetStringExtra("usuario");
            FindViewById<TextView>(Resource.Id.textView2).Text = Intent.GetStringExtra("tituloEvidencia");
            FindViewById<TextView>(Resource.Id.textView3).Text = Intent.GetStringExtra("StatusEvidencia");

            var WebView = FindViewById<WebView>(Resource.Id.webView1);
            var ImageView = FindViewById<ImageView>(Resource.Id.imageView1);

            HackAtHome.SAL.ServiceClient Detalle = new HackAtHome.SAL.ServiceClient();

            HackAtHome.Entities.EvidenceDetail DetalleEvidence = await Detalle.GetEvidenceByIDAsync(Token, idEv);

            string descripcion = "<style>body{color:lightgray;}</style>" + DetalleEvidence.Description;
         
            WebView.SetBackgroundColor(Color.DarkGray);
            WebView.LoadDataWithBaseURL(null, descripcion, "text/html", "utf-8", null);

            Koush.UrlImageViewHelper.SetUrlDrawable(ImageView, DetalleEvidence.Url);

        }
    }
}