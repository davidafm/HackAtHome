using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using HackAtHome.Entities;

namespace HackAtHome
{
    class Persistencia : Fragment
    {
        public List<Evidence> Evidencias { get; set; }

        public override void OnCreate(Bundle SavedInstance)
        {
            base.OnCreate(SavedInstance);
            RetainInstance = true;
        }

    

    }
}