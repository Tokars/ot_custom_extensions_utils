namespace OT.Extensions
{
    public static class IntegerExtension
    {
        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static byte[] SeparateDigits(byte n)
        {
            byte[] digits = new byte [3];

            var i = 0;

            Separate(n);

            return digits;

            void Separate(byte x)
            {
                if (x < 10)
                {
                    digits[i] = x;
                    i++;
                    return;
                }

                Separate((byte) (x / 10));
                digits[i] = (byte) (x % 10);
                i++;
            }
        }
    }
}