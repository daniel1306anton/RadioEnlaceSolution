using RadioEnlace.Shared.Dto;
using RadioEnlace.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Business
{
    public class CalculateProfileEarth
    {
        const double DeltaNAbs = (double)54.75;//Constant Abs Delta N
        const double DeltaN = (double)-54.75;//Constant Abs Delta N
        const double A = (double)0.25; //Superficie Terrestre
        const double B = (double)0.5;//Factor clima y humedad
        const double Fc = (double)5;//(Ghz) Frecuencia (factor de entrada)
        const double R = (double)0.99995;//Confiabilidad del radio enlace..
        const double Rorbit = (double)6373.02;//Radio terrestre
        double Fm;//Frecuencia de desvanecimiento
        double H1;//Altura antena 1
        double H2;//Altura antena 2

        double HA; //Altura terrestre + altura de antena INICIAL
        double HB; //Altura terrestre + altura de antena FINAL
        double Nd;//Número de datos
        double RKm;//Distancia de radio enlace
        double Ro; //Radio terrestre Om a k

        double NS;
        double ns;
        double K;
        double Ic;
        double d;//Distance (km)
        private void CalculateConst()
        {
            //Calculate Fm
            //30(Log(Rkm))+10(6*A*B*Fc)-10Log(1-R)-70
            var Fm1 = (30 * Math.Log10(RKm));
            var Fm2 = (10 * (6 * A * B * Fc));
            var Fm3 = (10 * Math.Log10(1 - R));
            Fm = Fm1 + Fm2 - Fm3 - 70;

            //Calculate NS
            //179.31 * Ln(DeltaNAbs / 7.32)
            NS = 179.31 * (Math.Log(DeltaNAbs / 7.32));

            //Calculate ns
            //1 + NS(10exp-6)
            ns = 1 + (NS * (Math.Pow(10, -6)));


            //Calculate K
            // 1 / (1 + ((Rorbit/ns) * Ln( NS / (NS + DeltaN) ) * NS * (10 exp -6)))
            var k1 = Rorbit / ns;
            var k2 = Math.Log(NS / (NS + DeltaN));
            var k3 = k1 * k2 * NS * Math.Pow(10, -6);
            var k4 = 1 + k3;

            K = 1 / k4;

            //Calculate Ro  = K * Rorbit
            Ro = K * Rorbit;

            //Calculate Ic
            //ABS(HA-HB) / (Nd-1)
            Ic = Math.Abs(HA - HB) / (Nd - 1);

            //Calculate d
            d = RKm;
        }
        internal List<EarthProfileDto> Execute(List<EarthProfileDto> earthProfileList, EarthProfileRequestDto earthProfileRequest)
        {
            H1 = earthProfileRequest.H1;
            H2 = earthProfileRequest.H2;
            Nd = earthProfileList.Count();
            RKm = earthProfileRequest.Distance;

            var indexMin = earthProfileList.Min(z => z.IndexFlow);
            var indexMax = earthProfileList.Max(z => z.IndexFlow);
            HA = H1 + (earthProfileList.Where(x => x.IndexFlow == indexMin).FirstOrDefault().Lm);
            HB = H2 + (earthProfileList.Where(x => x.IndexFlow == indexMax).FirstOrDefault().Lm);
            CalculateConst();

            var earthPorfileArray = earthProfileList.OrderBy(x => x.IndexFlow).ToArray();

            for (int i = 0; i < earthPorfileArray.Count(); i++)
            {
                //Calculate La(m)
                if(earthPorfileArray[i].IndexFlow == indexMin)
                {
                    earthPorfileArray[i].La = HA;
                }
                else
                {
                    earthPorfileArray[i].La = earthPorfileArray[i - 1].La + Ic;
                }

                //Calculate h(m)
                // ((d1 * d2) / 12.75) / K
                earthPorfileArray[i].H = ((earthPorfileArray[i].DistanceInitKm * earthPorfileArray[i].DistanceEndKm) / 12.75) / K;

                //Calculate Ht(m)
                //Lm + h;
                earthPorfileArray[i].Ht = earthPorfileArray[i].Lm + earthPorfileArray[i].H;

                //Calculate Rf
                // 17.32 * (RAIZ ( (d1*d2) / (Fc * d) ))
                var rf1 = (earthPorfileArray[i].DistanceInit * earthPorfileArray[i].DistanceEnd);
                var rf2 = rf1 / (Fc*d);
                earthPorfileArray[i].Rf = 17.32 * Math.Sqrt(rf2);

                //Calculate Zf
                //La - Rf
                earthPorfileArray[i].Zf = earthPorfileArray[i].La - earthPorfileArray[i].Rf;

                //Calculate e(m)
                //La - ht
                earthPorfileArray[i].E = earthPorfileArray[i].La - earthPorfileArray[i].Ht;

                //Calculate e/zf
                earthPorfileArray[i].EZf = earthPorfileArray[i].E / earthPorfileArray[i].Zf;
            }
            
            return earthPorfileArray.ToList();
        }

    }
}
