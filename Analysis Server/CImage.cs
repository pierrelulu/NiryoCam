using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace libImage
{
    public class ClImage
    {
        // on crée une classe C# avec pointeur sur l'objet C++
        // puis des static extern exportées de chaque méthode utile de la classe C++
        public IntPtr ClPtr;
        public double tempsTraitement;
        public Image source = null;
        //public Image gtruth = null;
        public Image result = null;

        public ClImage()
        {
            ClPtr = IntPtr.Zero;
        }

        ~ClImage()
        {
            if (ClPtr != IntPtr.Zero)
            {
                freeObjetLibDataPtr();
                ClPtr = IntPtr.Zero;
            }
        }


        // va-et-vient avec constructeur C#/C++
        // obligatoire dans toute nouvelle classe propre à l'application

        [DllImport("libImage.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr objetLib();

        public IntPtr objetLibPtr()
        {
            ClPtr = objetLib();
            return ClPtr;
        }

        [DllImport("libImage.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr objetLibDataImg(int nbChamps, IntPtr data, int stride, int nbLig, int nbCol);

        public IntPtr objetLibDataImgPtr(int nbChamps, IntPtr data, int stride, int nbLig, int nbCol)
        {
            ClPtr = objetLibDataImg(nbChamps, data, stride, nbLig, nbCol);
            return ClPtr;
        }

        [DllImport("libImage.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void freeObjetLibData(IntPtr ClPtr);
        
        public void freeObjetLibDataPtr()
        {
            if(ClPtr != IntPtr.Zero)
                freeObjetLibData(ClPtr);

            ClPtr = IntPtr.Zero;

        }

        [DllImport("libImage.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double valeurChamp(IntPtr pImg, int i);

        public double objetLibValeurChamp(int i)
        {
            return valeurChamp(ClPtr, i);
        }

        public static ClImage traiter(Image img)
        {
            ClImage Img = new ClImage();

            Img.source = img;
            //Img.gtruth = tuple.Item2;

            Bitmap sourceBMP = new Bitmap(img);
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            unsafe
            {
                BitmapData sourceBMPData = sourceBMP.LockBits(new Rectangle(0, 0, sourceBMP.Width, sourceBMP.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Img.objetLibDataImgPtr(0, sourceBMPData.Scan0, sourceBMPData.Stride, sourceBMP.Height, sourceBMP.Width);
                // 1 champ texte retour C++, le seuil auto
                sourceBMP.UnlockBits(sourceBMPData);
                Img.freeObjetLibDataPtr();
            }
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            Img.tempsTraitement = elapsedTime.TotalSeconds;
            Img.result = new Bitmap(sourceBMP);

            return Img;
        }
    }
}
