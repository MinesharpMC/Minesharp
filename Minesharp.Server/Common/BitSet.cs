namespace Minesharp.Server.Common;

public class BitSet
{
    private long[] words;
    private int wordsInUse;

    public BitSet()
    {
        words = new long[CreateWordIndex(64 - 1) + 1];
    }

    public BitSet(long[] values)
    {
        words = values;
        wordsInUse = values.Length;
    }

    public void Set(int index)
    {
        var wordIndex = CreateWordIndex(index);
        ExpandTo(wordIndex);

        words[wordIndex] |= 1L << index;
    }

    public long[] ToLongArray()
    {
        return words;
    }

    private void ExpandTo(int index)
    {
        var wordsRequired = index + 1;
        if (wordsInUse < wordsRequired)
        {
            EnsureCapacity(wordsRequired);
            wordsInUse = wordsRequired;
        }
    }

    private void EnsureCapacity(int wordsRequired)
    {
        if (words.Length < wordsRequired)
        {
            var request = Math.Max(2 * words.Length, wordsRequired);
            var current = new long[words.Length];
            Array.Copy(words, current, words.Length);
            words = new long[request];
            Array.Copy(current, words, current.Length);
        }
    }

    private static int CreateWordIndex(int index)
    {
        return index >> 6;
    }
}