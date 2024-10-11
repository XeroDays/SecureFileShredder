using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileShredder
{
    public class ShredderController
    {

        public void ShreddFile(string filePath, int passes,int bufferSize , BackgroundWorker worker, ref int progress)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[bufferSize];
                long fileLength = new FileInfo(filePath).Length;

                for (int i = 0; i < passes; i++)
                {
                    worker.ReportProgress(++progress);
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                    {
                        long position = 0;
                        while (position < fileLength)
                        {
                            rng.GetBytes(data);
                            fs.Write(data, 0, (int)Math.Min(data.Length, fileLength - position));
                            position += data.Length;
                        }
                    }
                }
            }
        }
    }
}
