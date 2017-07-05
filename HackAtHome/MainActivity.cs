using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Microsoft.WindowsAzure.MobileServices;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/iconoapp")]
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var ValidateButton = FindViewById<Button>(Resource.Id.btnValidate);
            var txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            var txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            ValidateButton.Click += (sender, e) =>
            {
                //SendEv(txtEmail.Text);
                Auth(txtEmail.Text, txtPassword.Text);
            };
        }

        public async void Auth(string email, string pass)
        {
            var Client = new HackAtHome.SAL.ServiceClient();
            var Res = await Client.AutenticateAsync(email, pass);

            if (Res.Status == Status.Success){

                SendEv(Intent.GetStringExtra("correo"));

                var ListEvidenciasActivity = new Android.Content.Intent(this, typeof(ListEvidenciasActivity));

                ListEvidenciasActivity.PutExtra("usuario", Res.FullName);
                ListEvidenciasActivity.PutExtra("token", Res.Token);
                ListEvidenciasActivity.PutExtra("correo", email);
                StartActivity(ListEvidenciasActivity);
            }
           
           
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

