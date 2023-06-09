namespace Diff
{
    public class DiffLogic
    {
        public static Dictionary<int, int> FindDifference(Diff diff)
        {
            var differences = new Dictionary<int, int>();
            var previousEqual = false;
            var previousPosition = 0;
            var offsetLength = 1;

            for (var ch = 0; ch < diff.Left.Length; ch++)
            {
                if (diff.Left[ch] != diff.Right[ch])
                {
                    if (previousEqual)
                    {
                        differences.Add(ch, 1);
                        previousPosition = ch;
                    }
                    else
                    {
                        offsetLength++;
                        differences[previousPosition] = offsetLength;
                    }
                    previousEqual = false;
                }
                else
                {
                    offsetLength = 1;
                    previousPosition = 0;
                    previousEqual = true;
                }
            }

            return differences;
        }
    }
}
