using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Media;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Entrenamiento_auditivo
{
    [Activity(Label = "Entrenamiento_auditivo", MainLauncher = true)]
    public class MainActivity : Activity
    {

        public static readonly IList<Double> Metrics = new ReadOnlyCollection<Double>
            (new List<Double> {});

        Button notaC;
        Button notaD;
        Button notaE;
        Button notaF;
        Button notaG;
        Button notaA;
        Button notaB;
        Button ejercicio;
        int ex;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            notaC = FindViewById<Button>(Resource.Id.btnDo);
            notaD = FindViewById<Button>(Resource.Id.btnRe);
            notaE= FindViewById<Button>(Resource.Id.btnMi);
            notaF = FindViewById<Button>(Resource.Id.btnFa);
            notaG = FindViewById<Button>(Resource.Id.btnSol);
            notaA = FindViewById<Button>(Resource.Id.btnLa);
            notaB = FindViewById<Button>(Resource.Id.btnSi);

            ejercicio = FindViewById<Button>(Resource.Id.btnEjercicio);

            ejercicio.Click += (o, e) => { ex = reproducir(); };

            notaC.Click += (o, e) => { respuesta(0);};
            notaD.Click += (o, e) => { respuesta(1); };
            notaE.Click += (o, e) => { respuesta(2); };
            notaF.Click += (o, e) => { respuesta(3); };
            notaG.Click += (o, e) => { respuesta(4); };
            notaA.Click += (o, e) => { respuesta(5); };
            notaB.Click += (o, e) => { respuesta(6); };
            

            void respuesta(int r)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);

                if(r == ex)
                {
                    alert.SetTitle("Respuesta Correcta");
                    alert.SetMessage("Su respuesta fue correcta.");
                }
                else
                {
                    alert.SetTitle("Respuesta Incorrecta");
                    alert.SetMessage("Su respuesta fue incorrecta.");
                }

                Dialog dialog = alert.Create();
                dialog.Show();
                
            }


            int reproducir() {
                Random ran = new Random();
                var duration = 2;
                var sampleRate = 8000;
                var numSamples = duration * sampleRate;
                var sample = new double[numSamples];
                Double freqOfTone = 0;
                byte[] generatedSnd = new byte[2 * numSamples];
                var tone = ran.Next(7);

                switch (tone)
                {
                    case 0:
                        freqOfTone = 261.626;
                        break;
                    case 1:
                        freqOfTone = 293.665;
                        break;
                    case 2:
                        freqOfTone = 329.628;
                        break;
                    case 3:
                        freqOfTone = 349.228;
                        break;
                    case 4:
                        freqOfTone = 391.995;
                        break;
                    case 5:
                        freqOfTone = 440.000;
                        break;
                    case 6:
                        freqOfTone = 493.88;
                        break;
                }

                for (int i = 0; i < numSamples; ++i)
                {
                    sample[i] = Math.Sin(2 * Math.PI * i / (sampleRate / freqOfTone));
                }

                int idx = 0;
                foreach (double dVal in sample)
                {
                    short val = (short)(dVal * 32767);
                    generatedSnd[idx++] = (byte)(val & 0x00ff);
                    generatedSnd[idx++] = (byte)((val & 0xff00) >> 8);
                }


                var track = new AudioTrack(global::Android.Media.Stream.Music, sampleRate, ChannelOut.Mono, Encoding.Pcm16bit, numSamples, AudioTrackMode.Static);
                track.Write(generatedSnd, 0, numSamples);
                track.Play();
                System.Threading.Thread.Sleep(1000);
                track.Release();

                return tone;
            }
        }
    }
}

