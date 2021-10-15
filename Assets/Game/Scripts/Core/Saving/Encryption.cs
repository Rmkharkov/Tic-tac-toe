namespace SavingCore
{
    using System.Text;

    public static class Encryption
    {
        private class EncrypStepData
        {
            public int step;
            public int key;

            public EncrypStepData(int step, int key)
            {
                this.step = step;
                this.key = key;
            }

            public void EncryptDecrypt(ref char c, int index)
            {
                c = index % step == 0 ? (char)(c ^ key) : c;
            }
        }
        //SHIFT is needed to remove specials symbols from encrypted string
        const int SHIFT = 32;

        static readonly private EncrypStepData[] encryptStepsData = new EncrypStepData[3]
            {
            new EncrypStepData(1, 7 ), new EncrypStepData( 2, 3 ), new EncrypStepData( 5, 64 )
            };

        public static string Encrypt(string textToEncrypt)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            int length = encryptStepsData.Length;
            int j = 0;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                for (j = 0; j < length; j++)
                {
                    encryptStepsData[j].EncryptDecrypt(ref c, i);
                }
                outSb.Append((char)(c + SHIFT));
            }

            return outSb.ToString();
        }

        public static string Decrypt(string textToDecrypt)
        {
            StringBuilder inSb = new StringBuilder(textToDecrypt);
            StringBuilder outSb = new StringBuilder(textToDecrypt.Length);
            char c;
            int length = encryptStepsData.Length;
            int j = 0;
            for (int i = 0; i < textToDecrypt.Length; i++)
            {
                c = (char)(inSb[i] - SHIFT);
                for (j = 0; j < length; j++)
                {
                    encryptStepsData[j].EncryptDecrypt(ref c, i);
                }
                outSb.Append(c);
            }

            return outSb.ToString();
        }

        private static char EncryptDecrypt(char charToEncrypt)
        {
            char c = charToEncrypt;
            int length = encryptStepsData.Length;
            for (int j = 0; j < length; j++)
            {
                encryptStepsData[j].EncryptDecrypt(ref c, 0);
            }
            return c;
        }
    }
}