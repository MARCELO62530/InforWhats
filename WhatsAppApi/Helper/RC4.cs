namespace WhatsAppApi.Helper
{
    public class RC4
    {
        private int i;
        private int j;
        private int[] s;

        public RC4(byte[] key, int drop)
        {
            s = new int[256];
            while (i < s.Length)
            {
                s[i] = i;
                i++;
            }
            j = 0;
            i = 0;
            while (i < 0x100)
            {
                j = ((j + key[i % key.Length]) + s[i]) & 0xff;
                Swap(s, i, j);
                i++;
            }
            i = j = 0;
            Cipher(new byte[drop]);
        }

        public void Cipher(byte[] data)
        {
            Cipher(data, 0, data.Length);
        }

        public void Cipher(byte[] data, int offset, int length)
        {
            for (int i = length; i > 0; i--)
            {
                this.i = (this.i + 1) & 0xff;
                j = (j + s[this.i]) & 0xff;
                Swap(s, this.i, j);
                int index = offset++;
                data[index] = (byte)(data[index] ^ s[(s[this.i] + s[j]) & 0xff]);
            }
        }

        private static void Swap<T>(T[] s, int i, int j)
        {
            T num = s[i];
            s[i] = s[j];
            s[j] = num;
        }
    }
}
